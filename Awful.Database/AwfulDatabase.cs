// <copyright file="AwfulDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.Core.Tools;
using Awful.Database.Entities;
using LiteDB;

namespace Awful.Database
{
    /// <summary>
    /// Awful Database.
    /// </summary>
    public class AwfulDatabase : IDatabase, IDisposable
    {
        private const string UserDB = nameof(UserDB);
        private const string OptionsDB = nameof(OptionsDB);
        private readonly IPlatformProperties properties;
        private readonly LiteDatabase db;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulDatabase"/> class.
        /// </summary>
        /// <param name="properties">Platform Properties.</param>
        public AwfulDatabase(IPlatformProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            this.properties = properties;
            this.db = new LiteDatabase(properties.DatabasePath + ".litedb");
        }

        /// <inheritdoc/>
        bool IDatabase.IsUserLoggedIn
        {
            get
            {
                var collection = this.db.GetCollection<UserAuth>(UserDB);
                var users = collection.FindAll().Count();
                if (users > 1)
                {
                    throw new Exception("Only one user account allowed to be logged in.");
                }

                return users == 1;
            }
        }

        /// <inheritdoc/>
        public SettingOptions GetAppSettings()
        {
            var collection = this.db.GetCollection<SettingOptions>(OptionsDB);
            var appSettings = collection.FindAll().ToList();
            var appSetting = appSettings.FirstOrDefault();
            if (appSetting != null)
            {
                return appSetting;
            }

            appSetting = new SettingOptions() { UseDarkMode = this.properties.IsDarkTheme };
            return appSetting;
        }

        /// <inheritdoc/>
        public bool SaveAppSettings(SettingOptions appSettings)
        {
            var collection = this.db.GetCollection<SettingOptions>(OptionsDB);
            return collection.Upsert(appSettings);
        }

        /// <inheritdoc/>
        public UserAuth GetLoggedInUser()
        {
            var collection = this.db.GetCollection<UserAuth>(UserDB);
            return collection.FindAll().FirstOrDefault();
        }

        /// <inheritdoc/>
        public bool SaveLoggedInUser(UserAuth userAuth)
        {
            var collection = this.db.GetCollection<UserAuth>(UserDB);
            return collection.Upsert(userAuth);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose DB.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.db.Dispose();
            }

            this.isDisposed = true;
        }
    }
}
