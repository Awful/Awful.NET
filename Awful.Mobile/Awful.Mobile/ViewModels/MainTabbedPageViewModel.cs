// <copyright file="MainTabbedPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Main Tabbed Page View Model.
    /// </summary>
    public class MainTabbedPageViewModel : MobileAwfulViewModel
    {
        private Xamarin.Forms.TabbedPage page;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainTabbedPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public MainTabbedPageViewModel(AwfulContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Loads TabbedPage into VM.
        /// </summary>
        /// <param name="page"><see cref="TabbedPage"/>.</param>
        public void LoadTabbedPage(Xamarin.Forms.TabbedPage page)
        {
            this.page = page;
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            if (!this.page.Children.Any())
            {
                this.RenderPages();
            }

            return base.OnLoad();
        }

        private static Xamarin.Forms.NavigationPage CreateNavigationPage (Xamarin.Forms.Page page, string glyph, string title, string fontFamily)
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
            foreach (var page in pages)
            {
                this.page.Children.Add(page);
            }
        }
    }
}
