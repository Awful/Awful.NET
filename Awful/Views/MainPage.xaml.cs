using System;
using Awful.Models.Forums;
using Awful.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Awful.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadAsync();
        }

        private async void AddOrRemoveFavorite(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            var forum = menuFlyoutItem?.CommandParameter as Forum;
            if (forum != null)
            {
                await ViewModel.AddOrRemoveFavorite(forum);
            }
        }

        private void MainForumListFull_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var forum = e.ClickedItem as Forum;
            //ViewModel.NavigateToThreadList(forum);
        }
    }
}
