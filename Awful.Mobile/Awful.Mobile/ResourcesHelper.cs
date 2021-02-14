// <copyright file="ResourcesHelper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Core.Tools;
using Awful.Database.Entities;
using Xamarin.Forms;

namespace Awful.Mobile
{
    /// <summary>
    /// Resource Helper.
    /// </summary>
    public static class ResourcesHelper
    {
        /// <summary>
        /// Background color.
        /// </summary>
        public const string DynamicBackgroundColor = nameof(DynamicBackgroundColor);

        /// <summary>
        /// Text Color.
        /// </summary>
        public const string DynamicTextColor = nameof(DynamicTextColor);

        /// <summary>
        /// Header Text Bar Color.
        /// </summary>
        public const string DynamicHeaderBarTextColor = nameof(DynamicHeaderBarTextColor);

        /// <summary>
        /// Header Background Color.
        /// </summary>
        public const string DynamicHeaderBackgroundColor = nameof(DynamicHeaderBackgroundColor);

        /// <summary>
        /// Forum Header Text Color.
        /// </summary>
        public const string ForumHeaderTextColor = nameof(ForumHeaderTextColor);

        /// <summary>
        /// Forum Background Color.
        /// </summary>
        public const string ForumBackground = nameof(ForumBackground);

        /// <summary>
        /// Set a dynamic resource.
        /// </summary>
        /// <param name="targetResourceName">Target Resource.</param>
        /// <param name="sourceResourceName">Value to change to.</param>
        public static void SetDynamicResource(string targetResourceName, string sourceResourceName)
        {
            if (!Application.Current.Resources.TryGetValue(sourceResourceName, out var value))
            {
                throw new InvalidOperationException($"key {sourceResourceName} not found in the resource dictionary");
            }

            Application.Current.Resources[targetResourceName] = value;
        }

        /// <summary>
        /// Set a dynamic resource.
        /// </summary>
        /// <typeparam name="T">Type Value.</typeparam>
        /// <param name="targetResourceName">Target Resource.</param>
        /// <param name="value">Value to change to.</param>
        public static void SetDynamicResource<T>(string targetResourceName, T value)
        {
            Application.Current.Resources[targetResourceName] = value;
        }

        /// <summary>
        /// Sets application to dark mode.
        /// </summary>
        public static void SetDarkMode()
        {
            Application.Current.UserAppTheme = OSAppTheme.Dark;
            SetDynamicResource(ForumBackground, "ForumHeaderTextColorLight");
            SetDynamicResource(ForumHeaderTextColor, "ForumBackgroundDark");

            SetDynamicResource(DynamicBackgroundColor, "BackgroundColorDark");
            SetDynamicResource(DynamicTextColor, "TextColorLight");

            SetDynamicResource(DynamicHeaderBackgroundColor, "HeaderBarBackgroundColorDark");
            SetDynamicResource(DynamicHeaderBarTextColor, "HeaderBarTextColorLight");
        }

        /// <summary>
        /// Sets application to light mode.
        /// </summary>
        public static void SetLightMode()
        {
            Application.Current.UserAppTheme = OSAppTheme.Light;
            SetDynamicResource(DynamicBackgroundColor, "BackgroundColorLight");
            SetDynamicResource(DynamicTextColor, "TextColorDark");

            SetDynamicResource(ForumBackground, "ForumHeaderTextColorDark");
            SetDynamicResource(ForumHeaderTextColor, "ForumBackgroundLight");

            SetDynamicResource(ForumHeaderTextColor, "BackgroundColorLight");
            SetDynamicResource(DynamicTextColor, "TextColorDark");

            SetDynamicResource(DynamicHeaderBackgroundColor, "HeaderBarBackgroundColorLight");
            SetDynamicResource(DynamicHeaderBarTextColor, "HeaderBarTextColorDark");
        }

        /// <summary>
        /// Sets application to custom theme.
        /// </summary>
        /// <param name="customTheme">The custom theme.</param>
        public static void SetCustomTheme(AppCustomTheme customTheme)
        {
            switch (customTheme)
            {
                case AppCustomTheme.None:
                    break;
                case AppCustomTheme.OLED:
                    SetDarkMode();
                    SetDynamicResource(DynamicBackgroundColor, "OLED_BackgroundColorDark");
                    SetDynamicResource(DynamicHeaderBackgroundColor, "OLED_BackgroundColorDark");
                    break;
                default:
                    break;
            }
        }

        public static void SetStatusBarColor(object color)
        {
            var platform = App.Container.Resolve<IPlatformProperties>();
            platform.SetStatusBarColor((Xamarin.Forms.Color)color);
        }
    }
}
