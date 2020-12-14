// <copyright file="MainPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Autofac;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Win.Controls;
using Awful.Win.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Awful.Win.Pages
{
    /// <summary>
    /// Main Page.
    /// </summary>
    public sealed partial class MainPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Container.Resolve<MainPageViewModel>();
            this.ViewModel.SetupThemeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the View Model.
        /// </summary>
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

        /// <summary>
        /// Gets the Content Frame.
        /// </summary>
        public Frame ContentFrame => this.contentFrame;

        private void MainPageNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                this.ViewModel.ItemSelectionCommand.ExecuteAsync("Settings").FireAndForgetSafeAsync(this.ViewModel.Error);
                return;
            }

            if (args.InvokedItem is string menu)
            {
                this.ViewModel.ItemSelectionCommand.ExecuteAsync(menu).FireAndForgetSafeAsync(this.ViewModel.Error);
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            this.mainPageNavView.IsBackEnabled = this.contentFrame.CanGoBack;

            if (this.contentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                this.mainPageNavView.SelectedItem = (NavigationViewItem)this.mainPageNavView.SettingsItem;
                this.mainPageNavView.Header = "Settings";
            }
            else if (this.contentFrame.SourcePageType != null)
            {
                var menuItemsSource = this.mainPageNavView.MenuItemsSource as ObservableCollection<AwfulMenuCategory>;
                if (menuItemsSource != null)
                {
                    var item = menuItemsSource.FirstOrDefault(p => p.Page == e.SourcePageType);
                    var menuItem = this.mainPageNavView.ContainerFromMenuItem(item);

                    this.mainPageNavView.SelectedItem = menuItem;
                    this.mainPageNavView.Header = item.Name;
                }
            }
        }

        private void MainPageNavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            this.On_BackRequested();
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            this.On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (!this.ContentFrame.CanGoBack)
            {
                return false;
            }

            // Don't go back if the nav pane is overlayed.
            if (this.mainPageNavView.IsPaneOpen &&
                (this.mainPageNavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 this.mainPageNavView.DisplayMode == NavigationViewDisplayMode.Minimal))
            {
                return false;
            }

            this.ContentFrame.GoBack();
            return true;
        }
    }
}
