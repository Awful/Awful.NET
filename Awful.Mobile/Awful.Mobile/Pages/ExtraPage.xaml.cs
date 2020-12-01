// <copyright file="ExtraPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Awful.Mobile.Controls;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    public partial class ExtraPage : BasePage
    {
        public ExtraPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ExtraPageCollectionView.ItemsSource = new List<ExtraPageItem>()
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
                    Glyph = "",
                    Title = "User Profile",
                },
                new ExtraPageItem()
                {
                    Type = "AboutCell",
                    Glyph = "",
                    Title = "About",
                },
            };
        }

        async void ExtraPageCollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
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
                                await App.PushPageAsync(new SAclopediaEntryListPage()).ConfigureAwait(false);
                                break;
                            case "LepersCell":
                                await App.PushPageAsync(new LepersPage()).ConfigureAwait(false);
                                break;
                            case "UserProfileCell":
                                await App.SetDetailPageAsync(new UserProfilePage(0)).ConfigureAwait(false);
                                break;
                            case "AboutCell":
                                break;
                        }
                    });
                }
            }

            this.ExtraPageCollectionView.SelectedItem = null;
        }
    }
}
