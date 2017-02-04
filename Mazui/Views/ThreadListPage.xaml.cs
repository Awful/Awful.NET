using AmazingPullToRefresh.Controls;
using Mazui.Core.Models.Threads;
using Mazui.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Mazui.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThreadListPage : Page
    {
        public ThreadListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel.ThreadView = ThreadPageView;
            ViewModel.MasterDetailViewControl = previewControl;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ResetPageCache();
            }
        }

        private void ResetPageCache()
        {
            var cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;
        }

        // strongly-typed view models enable x:bind
        public ThreadListPageViewModel ViewModel => this.DataContext as ThreadListPageViewModel;

        private async void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var thread = e.ClickedItem as Thread;
            await ViewModel.NavigateToThread(thread);
        }

        private async void GoToLastPage(object sender, RoutedEventArgs e)
        {
            var imageSource = sender as MenuFlyoutItem;
            var thread = imageSource?.CommandParameter as Thread;
            if (thread == null)
                return;
            ViewModel.Selected = thread;
            await ThreadPageView.LoadThread(thread, false, true);
            ViewModel.IsThreadSelectedAndLoaded = true;
        }

        private async void OpenInNewWindow(object sender, RoutedEventArgs e)
        {
            var imageSource = sender as MenuFlyoutItem;
            var thread = imageSource?.CommandParameter as Thread;
            if (thread == null)
                return;
            await ViewModel.OpenInNewWindow(thread);
        }

        private async void AddRemoveBookmark(object sender, RoutedEventArgs e)
        {
            var imageSource = sender as MenuFlyoutItem;
            var thread = imageSource?.CommandParameter as Thread;
            if (thread == null)
                return;
            await ViewModel.AddRemoveBookmark(thread);
        }

        private async void Unread(object sender, RoutedEventArgs e)
        {
            var imageSource = sender as MenuFlyoutItem;
            var thread = imageSource?.CommandParameter as Thread;
            if (thread == null)
                return;
            await ViewModel.UnreadThread(thread);
        }

        private void PullToRefreshExtender_RefreshRequested(object sender, RefreshRequestedEventArgs e)
        {
            ViewModel.Init();
        }
    }
}
