// <copyright file="SettingsViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.Core.Tools;
using Awful.Database;
using Awful.Database.Entities;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Settings View Model.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        private readonly SettingOptions appSettings;
        private readonly IPlatformProperties properties;
        private readonly IDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="properties">Platform Properties.</param>
        /// <param name="database">Database.</param>
        /// <param name="error">Error Handler.</param>
        /// <param name="navigation">Navigation Handler.</param>
        public SettingsViewModel(
            IPlatformProperties properties,
            IDatabase database,
            IAwfulErrorHandler error,
            IAwfulNavigationHandler navigation)
            : base(database, error, navigation)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.appSettings = database.GetAppSettings();
            this.Title = "Settings";
        }

        /// <summary>
        /// Gets the theme names.
        /// </summary>
        public List<string> CustomThemeNames
        {
            get
            {
                return Enum.GetNames(typeof(AppCustomTheme)).Select(b => b).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the device color theme.
        /// </summary>
        public AppCustomTheme CustomTheme
        {
            get
            {
                return this.appSettings.CustomTheme;
            }

            set
            {
                this.appSettings.CustomTheme = value;
                this.OnPropertyChanged(nameof(this.CustomTheme));
                this.database.SaveAppSettings(this.appSettings);
                this.SetTheme();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the system settings for themes,
        /// or let the user override them.
        /// </summary>
        public bool UseSystemThemeSettings
        {
            get => this.appSettings.UseSystemThemeSettings;

            set
            {
                this.appSettings.UseSystemThemeSettings = value;
                if (value)
                {
                    this.UseDarkMode = this.properties.IsDarkTheme;
                    this.CustomTheme = AppCustomTheme.None;
                }

                this.OnPropertyChanged(nameof(this.UseSystemThemeSettings));
                this.OnPropertyChanged(nameof(this.CanOverrideThemeSettings));
                this.database.SaveAppSettings(this.appSettings);
                this.SetTheme();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the system settings for themes,
        /// or let the user override them.
        /// </summary>
        public bool UseDarkMode
        {
            get => this.appSettings.UseDarkMode;

            set
            {
                this.appSettings.UseDarkMode = value;
                this.OnPropertyChanged(nameof(this.UseDarkMode));
                this.database.SaveAppSettings(this.appSettings);
                this.SetTheme();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can override theme settings.
        /// </summary>
        public bool CanOverrideThemeSettings => !this.UseSystemThemeSettings;

        private void SetTheme()
        {
            var darkMode = this.UseSystemThemeSettings ? this.properties.IsDarkTheme : this.UseDarkMode;
            if (!this.UseSystemThemeSettings && this.appSettings.CustomTheme != AppCustomTheme.None)
            {
                ResourcesHelper.SetCustomTheme(this.appSettings.CustomTheme);
                return;
            }

            if (darkMode)
            {
                ResourcesHelper.SetDarkMode();
            }
            else
            {
                ResourcesHelper.SetLightMode();
            }
        }
    }
}
