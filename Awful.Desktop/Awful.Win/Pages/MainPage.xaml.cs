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
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

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

            // Missing in WinUI???
            // Window.Current.SetTitleBar(this.AppTitleBar);

            // CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += (s, e) => this.UpdateAppTitle(s);
        }

        /// <summary>
        /// Gets the View Model.
        /// </summary>
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

        /// <summary>
        /// Gets the Content Frame.
        /// </summary>
        public Frame ContentFrame => this.contentFrame;

        /// <summary>
        /// Gets app title from system.
        /// </summary>
        /// <returns>String.</returns>
        public string GetAppTitleFromSystem()
        {
            return Windows.ApplicationModel.Package.Current.DisplayName;
        }

        private void MainPageNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.IsSelected)
            {
                // Clicked on an item that is already selected,
                // Avoid navigating to the same page again causing movement.
                return;
            }

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

        private void MainPageNavView_Loaded(object sender, RoutedEventArgs e)
        {
            var menuItemsSource = this.mainPageNavView.MenuItemsSource as ObservableCollection<AwfulMenuCategory>;
            var menuItem = menuItemsSource.First();
            var item = this.mainPageNavView.ContainerFromMenuItem(menuItem);
            this.mainPageNavView.SelectedItem = item;
            this.ViewModel.ItemSelectionCommand.ExecuteAsync(menuItem.Name).FireAndForgetSafeAsync();
        }

        //private void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        //{
        //    // ensure the custom title bar does not overlap window caption controls
        //    Thickness currMargin = this.AppTitleBar.Margin;
        //    var thickness = new Thickness
        //    {
        //        Left = currMargin.Left,
        //        Top = currMargin.Top,
        //        Right = coreTitleBar.SystemOverlayRightInset,
        //        Bottom = currMargin.Bottom,
        //    };
        //    this.AppTitleBar.Margin = thickness;
        //}

        //private void NavigationViewControl_PaneClosing(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewPaneClosingEventArgs args)
        //{
        //    this.UpdateAppTitleMargin(sender);
        //}

        //private void NavigationViewControl_PaneOpened(Microsoft.UI.Xaml.Controls.NavigationView sender, object args)
        //{
        //    this.UpdateAppTitleMargin(sender);
        //}

        //private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        //{
        //    Thickness currMargin = this.AppTitleBar.Margin;
        //    if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //    {
        //        this.AppTitleBar.Margin = new Thickness()
        //        {
        //            Left = sender.CompactPaneLength * 2,
        //            Top = currMargin.Top,
        //            Right = currMargin.Right,
        //            Bottom = currMargin.Bottom,
        //        };
        //    }
        //    else
        //    {
        //        this.AppTitleBar.Margin = new Thickness()
        //        {
        //            Left = sender.CompactPaneLength,
        //            Top = currMargin.Top,
        //            Right = currMargin.Right,
        //            Bottom = currMargin.Bottom,
        //        };
        //    }

        //    this.UpdateAppTitleMargin(sender);
        //    // UpdateHeaderMargin(sender);
        //}

        //private void UpdateAppTitleMargin(Microsoft.UI.Xaml.Controls.NavigationView sender)
        //{
        //    const int smallLeftIndent = 4, largeLeftIndent = 24;

        //    if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
        //    {
        //        this.AppTitle.TranslationTransition = new Vector3Transition();

        //        if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
        //                 sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //        {
        //            this.AppTitle.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
        //        }
        //        else
        //        {
        //            this.AppTitle.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
        //        }
        //    }
        //    else
        //    {
        //        Thickness currMargin = this.AppTitle.Margin;

        //        if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
        //                 sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //        {
        //            this.AppTitle.Margin = new Thickness()
        //            {
        //                Left = smallLeftIndent,
        //                Top = currMargin.Top,
        //                Right = currMargin.Right,
        //                Bottom = currMargin.Bottom,
        //            };
        //        }
        //        else
        //        {
        //            this.AppTitle.Margin = new Thickness()
        //            {
        //                Left = largeLeftIndent,
        //                Top = currMargin.Top,
        //                Right = currMargin.Right,
        //                Bottom = currMargin.Bottom,
        //            };
        //        }
        //    }
        //}
    }
}
