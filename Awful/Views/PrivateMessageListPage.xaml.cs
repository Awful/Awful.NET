using Awful.Parser.Models.Messages;
using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Awful.Views
{
    public sealed partial class PrivateMessageListPage : Page
    {
        public PrivateMessagesListViewModel ViewModel => this.DataContext as PrivateMessagesListViewModel;

        public PrivateMessageListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ThreadPageView.MasterDetailViewControl = previewControl;
            ViewModel.MasterDetailViewControl = previewControl;
            previewControl.SetMasterHeaderText("Private Messages");
            ViewModel.ThreadView = ThreadPageView;
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            previewControl.Loaded();
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            previewControl.Unloaded();
        }

        private void App_Resuming(object sender, object e)
        {
            previewControl.Loaded();
        }

        private void RefreshContainer_OnRefreshRequested(
            RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            if (!ViewModel.IsLoading)
                ViewModel.Init();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            previewControl.OnNavigated();
            if (ViewModel.Selected == null)
            {
                ViewModel.Init();
                await ThreadPageView.LoadBaseView();
            }
            App.ShellViewModel.BackNavigated -= NavigationService.BackRequested;
            App.ShellViewModel.BackNavigated += previewControl.NavigationManager_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            previewControl.FromNavigated();
            App.ShellViewModel.BackNavigated -= previewControl.NavigationManager_BackRequested;
            App.ShellViewModel.BackNavigated += NavigationService.BackRequested;
        }

        private async void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var thread = e.ClickedItem as PrivateMessage;
            if (thread == null)
                return;
            ViewModel.Selected = thread;
            await ThreadPageView.LoadPrivateMessage(thread);
            ViewModel.IsThreadSelectedAndLoaded = true;
        }
    }
}
