﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using static Awful.Tools.SettingsService;

namespace Awful.Tools
{
    public class SettingsHelper : ISettingsHelper
    {
        public ISettingsService Container(SettingsStrategies strategy)
        {
            return (strategy == SettingsStrategies.Local) ? Local : Roaming;
        }

        public bool Exists(string key, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return true;
            return Container(strategy).Exists(key);
        }

        public void Remove(string key, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            if (Container(strategy).Exists(key))
                Container(strategy).Remove(key);
        }

        public void Write<T>(string key, T value, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            Container(strategy).Write(key, value);
        }

        public T Read<T>(string key, T otherwise, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return otherwise;
            return Container(strategy).Read(key, otherwise);
        }

    }
}

namespace Awful.Tools
{
    public class SettingsService : ISettingsService
    {
        private static ISettingsService _local;
        public static ISettingsService Local => _local ?? (_local = Create(SettingsStrategies.Local));

        private static ISettingsService _roaming;
        public static ISettingsService Roaming => _roaming ?? (_roaming = Create(SettingsStrategies.Roam));

        /// <summary>
        /// Creates an <c>ISettingsService</c> object targeting the requested (optional) <paramref name="folderName"/>
        /// in the <paramref name="strategy"/> container.
        /// </summary>
        /// <param name="strategy">Roaming or Local</param>
        /// <param name="folderName">Name of the settings folder to use</param>
        /// <param name="createFolderIfNotExists"><c>true</c> to create the folder if it isn't already there, false otherwise.</param>
        /// <returns></returns>
        public static ISettingsService Create(SettingsStrategies strategy, string folderName = null, bool createFolderIfNotExists = true)
        {
            ApplicationDataContainer rootContainer;
            switch (strategy)
            {
                case SettingsStrategies.Local:
                    rootContainer = ApplicationData.Current.LocalSettings;
                    break;
                case SettingsStrategies.Roam:
                    rootContainer = ApplicationData.Current.RoamingSettings;
                    break;
                default:
                    throw new ArgumentException($"Unsupported Settings Strategy: {strategy}", nameof(strategy));
            }

            ApplicationDataContainer targetContainer = rootContainer;
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                try
                {
                    targetContainer = rootContainer.CreateContainer(folderName, createFolderIfNotExists ? ApplicationDataCreateDisposition.Always : ApplicationDataCreateDisposition.Existing);
                }
                catch (Exception)
                {
                    throw new KeyNotFoundException($"No folder exists named '{folderName}'");
                }
            }

            return new SettingsService(targetContainer);
        }

        protected ApplicationDataContainer Container { get; private set; }
        public IPropertySet Values { get; private set; }

        public IPropertyMapping Converters { get; set; } = new JsonMapping();

        private SettingsService(ApplicationDataContainer container)
        {
            Container = container;
            Values = container.Values;
        }

        public ISettingsService Open(string folderName, bool createFolderIfNotExists = true)
        {
            ApplicationDataContainer targetContainer;
            try
            {
                targetContainer = Container.CreateContainer(folderName, createFolderIfNotExists ? ApplicationDataCreateDisposition.Always : ApplicationDataCreateDisposition.Existing);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException($"No folder exists named '{folderName}'");
            }

            var service = new SettingsService(targetContainer);
            service.Converters = Converters;
            return service;
        }

        public bool Exists(string key) => Values.ContainsKey(key);

        public void Remove(string key)
        {
            if (Values.ContainsKey(key))
                Values.Remove(key);
            if (Container.Containers.ContainsKey(key))
                Container.DeleteContainer(key);
        }

        public void Clear(bool deleteSubContainers = true)
        {
            Values.Clear();
            if (deleteSubContainers)
            {
                foreach (var container in Container.Containers.ToArray())
                {
                    Container.DeleteContainer(container.Key);
                }
            }
        }

        const int MaxValueSize = 8000;

        public void Write<T>(string key, T value)
        {
            var type = typeof(T);
            if (value != null)
            {
                type = value.GetType();
            }
            var converter = Converters.GetConverter(type);
            var container = new ApplicationDataCompositeValue();
            var converted = converter.ToStore(value, type);
            if (converted != null)
            {
                var valueLength = converted.Length;
                if (valueLength > MaxValueSize)
                {
                    int count = (valueLength - 1) / MaxValueSize + 1;
                    container["Count"] = count;
                    for (int part = 0; part < count; part++)
                    {
                        string partValue = converted.Substring(part * MaxValueSize, Math.Min(MaxValueSize, valueLength));
                        container["Part" + part] = partValue;
                        valueLength = valueLength - MaxValueSize;
                    }
                }
                else
                    container["Value"] = converted;
            }
            if ((type != typeof(string) && !type.GetTypeInfo().IsValueType) || (type != typeof(T)))
            {
                container["Type"] = type.AssemblyQualifiedName;
            }
            Values[key] = container;
        }

        public T Read<T>(string key, T fallback = default(T))
        {
            try
            {
                if (Values.ContainsKey(key))
                {
                    var container = Values[key] as ApplicationDataCompositeValue;
                    var type = typeof(T);
                    if (container.ContainsKey("Type"))
                    {
                        type = Type.GetType((string)container["Type"]);
                    }
                    string value = null;
                    if (container.ContainsKey("Value"))
                    {
                        value = container["Value"] as string;
                    }
                    else if (container.ContainsKey("Count"))
                    {
                        int count = (int)container["Count"];
                        var sb = new StringBuilder(count * MaxValueSize);
                        for (int statePart = 0; statePart < count; statePart++)
                        {
                            sb.Append(container["Part" + statePart]);
                        }
                        value = sb.ToString();
                    }
                    var converter = Converters.GetConverter(type);
                    var converted = (T)converter.FromStore(value, type);
                    return converted;
                }
                return fallback;
            }
            catch
            {
                return fallback;
            }
        }
    }

    public enum SettingsStrategies { Local, Roam, Temp }

    public class JsonMapping : IPropertyMapping
    {
        protected IStoreConverter jsonConverter = new JsonConverter();
        public IStoreConverter GetConverter(Type type) => this.jsonConverter;
    }

    public class JsonConverter : IStoreConverter
    {
        public object FromStore(string value, Type type) => JsonConvert.DeserializeObject(value, type);
        public string ToStore(object value, Type type) => JsonConvert.SerializeObject(value, Formatting.None);
    }


    public interface ISettingsHelper
    {
        ISettingsService Container(SettingsStrategies strategy);
        bool Exists(string key, SettingsStrategies strategy = SettingsStrategies.Local);
        T Read<T>(string key, T otherwise, SettingsStrategies strategy = SettingsStrategies.Local);
        void Remove(string key, SettingsStrategies strategy = SettingsStrategies.Local);
        void Write<T>(string key, T value, SettingsStrategies strategy = SettingsStrategies.Local);
    }

    public interface IStoreConverter
    {
        string ToStore(object value, Type type);
        object FromStore(string value, Type type);

    }
    public interface IPropertyMapping
    {

        IStoreConverter GetConverter(Type type);
    }
    public interface ISettingsService
    {
        IPropertyMapping Converters { get; set; }
        IPropertySet Values { get; }
        bool Exists(string key);
        T Read<T>(string key, T fallback = default(T));
        void Remove(string key);
        void Write<T>(string key, T value);
        ISettingsService Open(string folderName, bool createFolderIsNotExists = true);
        void Clear(bool deleteSubContainers = true);
    }
}