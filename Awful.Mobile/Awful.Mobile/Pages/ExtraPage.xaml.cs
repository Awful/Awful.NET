// <copyright file="ExtraPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Autofac;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Extra Page.
    /// Used to link out to other pages in the app.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraPage : BasePage
    {
        IAwfulNavigation navigation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtraPage"/> class.
        /// </summary>
        public ExtraPage()
        {
            this.InitializeComponent();
            this.navigation = App.Container.Resolve<IAwfulNavigation>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var itemList = new List<ExtraPageItem>()
            {
                new ExtraPageItem()
                {
                    Type = "SAclopediaCell",
                    Glyph = "",
                    Title = "SAclopedia",
                    Description = "The Something Awful Encyclopedia.",
                },
                new ExtraPageItem()
                {
                    Type = "LepersCell",
                    Glyph = "",
                    Title = "Leper's Colony",
                    Description = "List of banned users and the reason they were banned.",
                },
                new ExtraPageItem()
                {
                    Type = "UserProfileCell",
                    Glyph = "",
                    Title = "User Profile",
                    Description = "Go straight to the currently logged in users profile.",
                },
                new ExtraPageItem()
                {
                    Type = "AcknowledgmentsCell",
                    Glyph = "",
                    Title = "Acknowledgments",
                    Description = "All of the cool libraries and people who helped make this app possible.",
                },
            };

#if DEBUG
            itemList.Add(new ExtraPageItem()
            {
                Type = "DebugCell",
                Glyph = "",
                Title = "Debug",
                Description = "Scratch page with a bunch of debug stuff. If you can see this and it ain't a debug build, something got screwed up!",
            });
#endif

            this.ExtraPageCollectionView.ItemsSource = itemList;
        }

        /// <summary>
        /// Handle Extra Page Collection View Item Selection.
        /// Navigate to new page.
        /// </summary>
        /// <param name="sender"><see cref="object"/>.</param>
        /// <param name="e"><see cref="SelectionChangedEventArgs"/>.</param>
        private void ExtraPageCollectionView_SelectionChanged(object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selection = e.CurrentSelection[0];
                if (selection is ExtraPageItem item)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        switch (item.Type)
                        {
                            case "SAclopediaCell":
                                await this.navigation.PushPageAsync(new SAclopediaEntryListPage()).ConfigureAwait(false);
                                break;
                            case "LepersCell":
                                await this.navigation.PushPageAsync(new LepersPage()).ConfigureAwait(false);
                                break;
                            case "UserProfileCell":
                                await this.navigation.PushDetailPageAsync(new UserProfilePage(0)).ConfigureAwait(false);
                                break;
                            case "AcknowledgmentsCell":
                                await this.navigation.PushPageAsync(new AcknowledgmentsPage()).ConfigureAwait(false);
                                break;
                            case "DebugCell":
                                await this.navigation.PushPageAsync(new DebugPage()).ConfigureAwait(false);
                                break;
                        }
                    });
                }
            }

            this.ExtraPageCollectionView.SelectedItem = null;
        }
    }
}
