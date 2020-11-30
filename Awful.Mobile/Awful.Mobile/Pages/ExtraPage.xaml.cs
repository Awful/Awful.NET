// <copyright file="ExtraPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    public partial class ExtraPage : BasePage
    {
        public ExtraPage()
        {
            InitializeComponent();
        }

        async void ImageCell_Tapped(System.Object sender, System.EventArgs e)
        {
            if (sender is Cell cell)
            {
                switch (cell.ClassId)
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
            }
        }
    }
}
