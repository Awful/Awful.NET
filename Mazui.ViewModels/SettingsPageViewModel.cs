﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Mazui.ViewModels
{
    public class SettingsPageViewModel : MazuiViewModel
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : MazuiViewModel
    {
        readonly Services.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsService.Instance;
        }

        public bool UseDarkThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Dark); }
            set { _settings.AppTheme = value ? ApplicationTheme.Dark : ApplicationTheme.Light; base.RaisePropertyChanged(); }
        }

    }

    public class AboutPartViewModel : MazuiViewModel
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }

        public Uri RateMe => new Uri("https://somethingawful.com");
    }
}