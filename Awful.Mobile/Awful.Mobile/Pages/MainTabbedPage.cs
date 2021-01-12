// <copyright file="MainTabbedPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.Tools;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Main Tabbed Page.
    /// </summary>
    public class MainTabbedPage : Xamarin.Forms.TabbedPage
    {
        private MainTabbedPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainTabbedPage"/> class.
        /// </summary>
        public MainTabbedPage()
        {
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.RenderPages();
            //this.BindingContext = this.vm = App.Container.Resolve<MainTabbedPageViewModel>();
            //this.vm.SetupThemeAsync().ConfigureAwait(false);
            //this.vm.LoadTabbedPage(this);
        }

        private static Xamarin.Forms.NavigationPage CreateNavigationPage(Xamarin.Forms.Page page, string glyph, string title, string fontFamily)
        {
            Xamarin.Forms.NavigationPage navigationPage = new Xamarin.Forms.NavigationPage(page);
            // navigationPage.On<iOS>().SetPrefersLargeTitles(true);
            navigationPage.IconImageSource = new FontImageSource()
            {
                FontFamily = fontFamily,
                Glyph = glyph,
                Size = 24,
            };
            navigationPage.Title = title;
            return navigationPage;
        }

        private void RenderPages()
        {
            List<Xamarin.Forms.NavigationPage> pages = new List<Xamarin.Forms.NavigationPage>();
            pages.Add(CreateNavigationPage(new ForumListPage(), "", "Forums", "FontAwesomeSolid"));
            pages.Add(CreateNavigationPage(new BookmarksPage(), "", "Bookmarks", "FontAwesomeRegular"));
            pages.Add(CreateNavigationPage(new PrivateMessagesPage(), "", "Messages", "FontAwesomeRegular"));

            pages.Add(CreateNavigationPage(new SettingsPage(), "", "Settings", "FontAwesomeSolid"));
            pages.Add(CreateNavigationPage(new ExtraPage(), "", "More", "FontAwesomeSolid"));
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var page in pages)
                {
                    this.Children.Add(page);
                }
            });
        }
    }
}