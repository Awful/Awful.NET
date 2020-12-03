// <copyright file="ExtraPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Extra Page.
    /// Used to link out to other pages in the app.
    /// </summary>
    public partial class ExtraPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtraPage"/> class.
        /// </summary>
        public ExtraPage()
        {
            this.InitializeComponent();
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
                },
                new ExtraPageItem()
                {
                    Type = "LepersCell",
                    Glyph = "",
                    Title = "Leper's Colony",
                },
                new ExtraPageItem()
                {
                    Type = "UserProfileCell",
                    Glyph = "",
                    Title = "User Profile",
                },
                new ExtraPageItem()
                {
                    Type = "AboutCell",
                    Glyph = "",
                    Title = "About",
                },
            };

#if DEBUG
            itemList.Add(new ExtraPageItem()
            {
                Type = "DebugCell",
                Glyph = "",
                Title = "Debug",
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
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        switch (item.Type)
                        {
                            case "SAclopediaCell":
                                await MobileAwfulViewModel.PushPageAsync(new SAclopediaEntryListPage()).ConfigureAwait(false);
                                break;
                            case "LepersCell":
                                await MobileAwfulViewModel.PushPageAsync(new LepersPage()).ConfigureAwait(false);
                                break;
                            case "UserProfileCell":
                                await MobileAwfulViewModel.PushDetailPageAsync(new UserProfilePage(0)).ConfigureAwait(false);
                                break;
                            case "AboutCell":
                                break;
                            case "DebugCell":
                                await MobileAwfulViewModel.PushPageAsync(new DebugPage()).ConfigureAwait(false);
                                break;
                        }
                    });
                }
            }

            this.ExtraPageCollectionView.SelectedItem = null;
        }
    }
}
