using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazui.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mazui.Views
{
    public class BasePage : Page
    {
        Services.SettingsService _settings;

        public BasePage()
        {
            _settings = Services.SettingsService.Instance;
            InitializeSettings();
            //_settings.ChangedAppTheme += NewTestHandle();
            // _settings.ChangedAppThemeHandler += new SettingsService.ChangedAppTheme(TestHandle);
        }

        private void TestHandle()
        {
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            if (_settings.AppTheme == ApplicationTheme.Dark)
            {
                base.RequestedTheme = Windows.UI.Xaml.ElementTheme.Dark;
                this.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///Styles/Dark.xaml")
                });
            }
            else
            {
                base.RequestedTheme = Windows.UI.Xaml.ElementTheme.Light;
                this.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///Styles/Light.xaml")
                });
            }
        }
    }
}
