// <copyright file="BookmarksPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Autofac;
using Awful.Windows.Bookmarks.ViewModels;
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

namespace Awful.Windows.Bookmarks.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookmarksPage : Page
    {
        public BookmarksViewModel ViewModel => this.DataContext as BookmarksViewModel;

        public BookmarksPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Container.Resolve<BookmarksViewModel>();
        }

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.ViewModel.LoadBookmarksAsync(true).ConfigureAwait(false);
        }

        private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using (var refreshCompletionDeferral = args.GetDeferral())
            {
                await this.ViewModel.RefreshBookmarksAsync().ConfigureAwait(false);
            }
        }
    }
}
