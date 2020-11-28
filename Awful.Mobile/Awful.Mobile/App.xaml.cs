// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile
{
    /// <summary>
    /// Awful App.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Autofac Container.
        /// </summary>
        public static IContainer Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="builder">Container Builder.</param>
        public App(ContainerBuilder builder)
        {
            Device.SetFlags(new string[] { "Shell_UWP_Experimental", "AppTheme_Experimental", "CollectionView_Experimental", "Shapes_Experimental" });
            this.InitializeComponent();
            Container = Awful.UI.AwfulContainer.BuildContainer(builder);

            var context = App.Container.Resolve<AwfulContext>();
            var user = context.DoesUsersExistAsync().Result;
            if (!user)
            {
                App.Current.MainPage = new LoginPage();
            }
            else
            {
                SetMainAppPage();
            }
        }

        public static void SetMainAppPage()
        {
            var context = App.Container.Resolve<AwfulContext>();
            var settings = new SettingsAction(context);

            var options = context.SettingOptionsItems.FirstOrDefaultAsync().Result;
            if (options != null)
            {
                settings.SetAppTheme(options.DeviceColorTheme);
            }
            else
            {
                settings.SetAppTheme(Webview.Entities.Themes.DeviceColorTheme.Unspecified);
            }

            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet)
            {
                App.Current.MainPage = new MainPage();
            }
            else
            {
                App.Current.MainPage = new MasterPage();
            }
        }

        /// <summary>
        /// Push Modal Page to current navigation stack.
        /// If on tablet, pushes on top of Detail.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>Task.</returns>
        public static async Task PushModalAsync(Page page)
        {
            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                Page mp = (Page)flyout.Detail;
                await mp.Navigation.PushModalAsync(page).ConfigureAwait(false);
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                await tabbedPage.CurrentPage.Navigation.PushModalAsync(page).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Push Page to current navigation stack.
        /// If on tablet, pushes on top of Master.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>Task.</returns>
        public static async Task PushPageAsync(Page page)
        {
            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                if (flyout.Flyout is NavigationPage navPage)
                {
                    if (navPage.CurrentPage is TabbedPage tb)
                    {
                        await tb.CurrentPage.Navigation.PushAsync(page).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                await tabbedPage.CurrentPage.Navigation.PushAsync(page).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Set Detail Page for Master Detail if on Tablet, else push navigation.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>Task.</returns>
        public static async Task SetDetailPageAsync(Page page)
        {
            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet)
            {
                FlyoutPage flyout = (FlyoutPage)App.Current.MainPage;
                flyout.Detail = new NavigationPage(page);
                if (flyout.FlyoutLayoutBehavior == FlyoutLayoutBehavior.Popover)
                {
                    flyout.IsPresented = false;
                }
            }
            else
            {
                TabbedPage tabbedPage = (TabbedPage)App.Current.MainPage;
                await tabbedPage.CurrentPage.Navigation.PushAsync(page).ConfigureAwait(false);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
