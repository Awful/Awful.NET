// <copyright file="AwfulMobileNavigation.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.Pages;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview.Entities.Themes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Awful Mobile Navigation.
    /// </summary>
    public class AwfulMobileNavigation : IAwfulNavigation
    {
        private IPlatformProperties platformProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulMobileNavigation"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public AwfulMobileNavigation(IAwfulContext context)
        {
            this.SettingsAction = new SettingsAction(context);
            this.platformProperties = App.Container.Resolve<IPlatformProperties>();
        }

        /// <summary>
        /// Gets a value indicating whether the device
        /// is a "large" format (Tablet, Desktop)
        /// or not.
        /// </summary>
        public static bool IsLargeDevice => false;

        /// <summary>
        /// Gets the Settings Actions. Used to handle settings.
        /// </summary>
        public SettingsAction SettingsAction { get; internal set; }

        /// <inheritdoc/>
        public Task DisplayAlertAsync(string title, string message)
        {
            Device.BeginInvokeOnMainThread(async () => await App.Current.MainPage.DisplayAlert(title, message, "Close").ConfigureAwait(false));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<string> DisplayPromptAsync(string title, string message, string placeholder = "Text", string initialValue = "")
        {
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                string result = await App.Current.MainPage.DisplayPromptAsync(title, message, placeholder: placeholder, initialValue: initialValue).ConfigureAwait(false);
                tcs.TrySetResult(result);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public async Task LogoutAsync(IAwfulContext context, IPlatformProperties properties)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Log Out", "Are you sure you want to log out?", "Yep", "Nope").ConfigureAwait(false);
            if (answer)
            {
                System.IO.File.Delete(this.platformProperties.CookiePath);
                context.ResetDatabase();
                Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
            }
        }

        /// <inheritdoc/>
        public Task PopModalAsync()
        {
            if (IsLargeDevice)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                Page mp = (Page)flyout.Detail;
                Device.BeginInvokeOnMainThread(async () => await mp.Navigation.PopModalAsync().ConfigureAwait(false));
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                Device.BeginInvokeOnMainThread(async () => await tabbedPage.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PushDetailPageAsync(object objPage)
        {
            Page page = (Page)objPage;
            if (IsLargeDevice)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                flyout.Detail = new NavigationPage(page);
                if (flyout.FlyoutLayoutBehavior == FlyoutLayoutBehavior.Popover)
                {
                    Device.BeginInvokeOnMainThread(() => flyout.IsPresented = false);
                }
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                Device.BeginInvokeOnMainThread(async () => await tabbedPage.CurrentPage.Navigation.PushAsync(page).ConfigureAwait(false));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PushModalAsync(object objPage)
        {
            Page page = (Page)objPage;
            if (IsLargeDevice)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                Page mp = (Page)flyout.Detail;
                Device.BeginInvokeOnMainThread(async () => await mp.Navigation.PushModalAsync(page).ConfigureAwait(false));
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                Device.BeginInvokeOnMainThread(async () => await tabbedPage.CurrentPage.Navigation.PushModalAsync(page).ConfigureAwait(false));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PushPageAsync(object objPage)
        {
            Page page = (Page)objPage;
            if (IsLargeDevice)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                if (flyout.Flyout is NavigationPage navPage)
                {
                    if (navPage.CurrentPage is TabbedPage tb)
                    {
                        Device.BeginInvokeOnMainThread(async () => await tb.CurrentPage.Navigation.PushAsync(page).ConfigureAwait(false));
                    }
                }
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                Device.BeginInvokeOnMainThread(async () => await tabbedPage.CurrentPage.Navigation.PushAsync(page).ConfigureAwait(false));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task RefreshForumPageAsync()
        {
            ForumThreadListPageViewModel vm = null;
            if (IsLargeDevice)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                Page mp = (Page)flyout.Detail;
                vm = mp.BindingContext as ForumThreadListPageViewModel;
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                var navigationPage = tabbedPage.CurrentPage as NavigationPage;
                if (navigationPage != null)
                {
                    vm = navigationPage.CurrentPage.BindingContext as ForumThreadListPageViewModel;
                }
            }

            if (vm != null)
            {
                await vm.RefreshForums().ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task RefreshPostPageAsync()
        {
            ForumThreadPageViewModel vm = null;
            if (IsLargeDevice)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                Page mp = (Page)flyout.Detail;
                vm = mp.BindingContext as ForumThreadPageViewModel;
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                var navigationPage = tabbedPage.CurrentPage as NavigationPage;
                if (navigationPage != null)
                {
                    vm = navigationPage.CurrentPage.BindingContext as ForumThreadPageViewModel;
                }
            }

            if (vm != null)
            {
                await vm.RefreshThreadAsync().ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public void SetMainAppPage()
        {
            if (IsLargeDevice)
            {
                App.Current.MainPage = new MainPage();
            }
            else
            {
                App.Current.MainPage = new MainTabbedPage();
            }
        }

        /// <inheritdoc/>
        public Task SetMainAppPageAsync()
        {
            Device.BeginInvokeOnMainThread(() => SetMainAppPage());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetupThemeAsync()
        {
            var options = await this.SettingsAction.LoadSettingOptionsAsync().ConfigureAwait(false);
            this.SetTheme(options);
        }

        /// <summary>
        /// Setup Theme.
        /// </summary>
        /// <param name="options">Options.</param>
        public void SetTheme(SettingOptions options)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => {
                var darkMode = options.UseSystemThemeSettings ? this.platformProperties.IsDarkTheme : options.UseDarkMode;
                if (!options.UseSystemThemeSettings && options.CustomTheme != AppCustomTheme.None)
                {
                    ResourcesHelper.SetCustomTheme(options.CustomTheme);
                    return;
                }

                if (darkMode)
                {
                    ResourcesHelper.SetDarkMode();
                }
                else
                {
                    ResourcesHelper.SetLightMode();
                }
            });
        }
    }
}
