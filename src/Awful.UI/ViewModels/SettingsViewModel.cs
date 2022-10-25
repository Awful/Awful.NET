// <copyright file="SettingsViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.UI.Actions;
using Awful.UI.Entities;

namespace Awful.UI.ViewModels
{
    public class SettingsViewModel : AwfulViewModel
    {
        private SettingsAction settingActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public SettingsViewModel(IServiceProvider services)
            : base(services)
        {
            settingActions = new SettingsAction(Context);
            Settings = Context.GetAppSettings();
        }

        /// <summary>
        /// Gets the setting options.
        /// </summary>
        protected SettingOptions Settings { get; private set; }

        /// <summary>
        /// Gets the theme names.
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public List<string> CustomThemeNames
#pragma warning restore CA1822 // Mark members as static
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
                return Settings.CustomTheme;
            }

            set
            {
                Settings.CustomTheme = value;
                OnPropertyChanged(nameof(CustomTheme));
                Context.SaveAppSettings(Settings);
                SetTheme();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the system settings for themes,
        /// or let the user override them.
        /// </summary>
        public bool UseSystemThemeSettings
        {
            get => Settings.UseSystemThemeSettings;

            set
            {
                Settings.UseSystemThemeSettings = value;
                if (value)
                {
                    UseDarkMode = PlatformServices.IsDarkTheme;
                    CustomTheme = AppCustomTheme.None;
                }

                OnPropertyChanged(nameof(UseSystemThemeSettings));
                OnPropertyChanged(nameof(CanOverrideThemeSettings));
                Context.SaveAppSettings(Settings);
                SetTheme();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the system settings for themes,
        /// or let the user override them.
        /// </summary>
        public bool UseDarkMode
        {
            get => Settings.UseDarkMode;

            set
            {
                Settings.UseDarkMode = value;
                OnPropertyChanged(nameof(UseDarkMode));
                Context.SaveAppSettings(Settings);
                SetTheme();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can override theme settings.
        /// </summary>
        public bool CanOverrideThemeSettings => !UseSystemThemeSettings;

        /// <summary>
        /// Gets or sets a value indicating whether to enable background tasks.
        /// </summary>
        public bool EnableBackgroundTasks
        {
            get
            {
                return Settings.EnableBackgroundTasks;
            }

            set
            {
                Settings.EnableBackgroundTasks = value;
                OnPropertyChanged(nameof(EnableBackgroundTasks));
                Context.SaveAppSettings(Settings);
            }
        }

        /// <summary>
        /// Sets the theme for the running app.
        /// </summary>
        public virtual void SetTheme()
        {

        }
    }
}
