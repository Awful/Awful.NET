// <copyright file="AuthorizationPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Views
{
    public class AuthorizationPage : ContentPage
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext is AwfulViewModel vm)
            {
                await vm.SetupVM().ConfigureAwait(false);
            }
        }
    }
}