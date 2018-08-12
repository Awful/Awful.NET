using System;
using Awful.Parser.Models.Forums;
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
            App.ShellViewModel.Header = "Awful Forums Reader";
            await ViewModel.LoadAsync();
            ForumViewSource.Source = ViewModel.ForumGroupList;
            var collectionGroups = ForumViewSource.View.CollectionGroups;
            ((ListViewBase)this.semanticZoom.ZoomedOutView).ItemsSource = collectionGroups;
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
            ViewModel.NavigateToThreadList(forum);
        }

        private async void RefreshContainer_OnRefreshRequested(
            RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            await ViewModel.LoadAsync(true);
        }
    }
}
