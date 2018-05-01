using System;
using Awful.Services;
using Awful.ViewModels;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Awful.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
            App.ShellViewModel = ViewModel;
            NavigationService.Navigated += Navigated_Shell;
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private void Navigated_Shell(object sender, NavigationEventArgs e)
        {
            ViewModel.CanGoBack = NavigationService.CanGoBack;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox")
            {
                //ViewModeService.Instance.CollapseNavigationViewToBurger();
                //TitleBarHelper.Instance.TitleVisibility = Visibility.Collapsed;
            }
        }
    }
}
