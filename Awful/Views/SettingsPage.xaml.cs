using System;

using Awful.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Awful.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel => this.DataContext as SettingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();
            App.ShellViewModel.Header = "Settings";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
