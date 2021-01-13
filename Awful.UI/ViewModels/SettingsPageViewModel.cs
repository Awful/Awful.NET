// <copyright file="SettingsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Mobile Settings Page View Model.
    /// </summary>
    public class SettingsPageViewModel : AwfulViewModel
    {
        private SettingsAction settingActions;
        protected SettingOptions settings;
        private IPlatformProperties platformProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="platformProperties">Platform Properties.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public SettingsPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IPlatformProperties platformProperties, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.platformProperties = platformProperties;
            this.settings = this.Context.GetAppSettings();
            this.settingActions = new SettingsAction(context);
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
                return this.settings.CustomTheme;
            }

            set
            {
                this.settings.CustomTheme = value;
                this.OnPropertyChanged(nameof(this.CustomTheme));
                this.Context.SaveAppSettings(this.settings);
                this.SetTheme();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the system settings for themes,
        /// or let the user override them.
        /// </summary>
        public bool UseSystemThemeSettings
        {
            get => this.settings.UseSystemThemeSettings;

            set
            {
                this.settings.UseSystemThemeSettings = value;
                if (value)
                {
                    this.UseDarkMode = this.platformProperties.IsDarkTheme;
                    this.CustomTheme = AppCustomTheme.None;
                }

                this.OnPropertyChanged(nameof(this.UseSystemThemeSettings));
                this.OnPropertyChanged(nameof(this.CanOverrideThemeSettings));
                this.Context.SaveAppSettings(this.settings);
                this.SetTheme();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the system settings for themes,
        /// or let the user override them.
        /// </summary>
        public bool UseDarkMode
        {
            get => this.settings.UseDarkMode;

            set
            {
                this.settings.UseDarkMode = value;
                this.OnPropertyChanged(nameof(this.UseDarkMode));
                this.Context.SaveAppSettings(this.settings);
                this.SetTheme();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can override theme settings.
        /// </summary>
        public bool CanOverrideThemeSettings => !this.UseSystemThemeSettings;

        /// <summary>
        /// Gets or sets a value indicating whether to enable background tasks.
        /// </summary>
        public bool EnableBackgroundTasks
        {
            get
            {
                return this.settings.EnableBackgroundTasks;
            }

            set
            {
                this.settings.EnableBackgroundTasks = value;
                this.OnPropertyChanged(nameof(this.EnableBackgroundTasks));
                this.Context.SaveAppSettings(this.settings);
            }
        }

        /// <summary>
        /// Gets the login page command.
        /// </summary>
        public AwfulAsyncCommand LoginPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    await this.Navigation.LogoutAsync(this.Context, this.platformProperties).ConfigureAwait(false);
                },
                    null,
                    this.Error);
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
        }

        public virtual void SetTheme()
        {
        }

        private async Task SaveSettingsAsync()
        {
            await this.settingActions.SaveSettingOptionsAsync(this.settings).ConfigureAwait(false);
        }
    }
}
