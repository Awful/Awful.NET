using AmazingPullToRefresh.Controls;
using Mazui.Core.Models.Messages;
using Mazui.ViewModels;
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
    public sealed partial class PrivateMessageListPage : Page
    {
        public PrivateMessageListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void ResetPageCache()
        {
            var cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested += previewControl.NavigationManager_BackRequested;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested -= previewControl.NavigationManager_BackRequested;
            if (e.NavigationMode == NavigationMode.Back)
            {
                ResetPageCache();
            }
        }


        // strongly-typed view models enable x:bind
        public PrivateMessagesListViewModel ViewModel => this.DataContext as PrivateMessagesListViewModel;

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var thread = e.ClickedItem as PrivateMessage;
            if (thread == null)
                return;
            await PrivateMessageView.LoadPrivateMessage(thread);
            ViewModel.IsThreadSelectedAndLoaded = true;
        }

        private void PullToRefreshExtender_RefreshRequested(object sender, RefreshRequestedEventArgs e)
        {
            ViewModel.Refresh();
        }
    }
}
