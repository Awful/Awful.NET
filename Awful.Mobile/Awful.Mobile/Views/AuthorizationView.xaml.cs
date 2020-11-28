// <copyright file="AuthorizationView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Awful.Mobile.Pages;
using Xamarin.Forms;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Authorization View.
    /// </summary>
    public partial class AuthorizationView : ContentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationView"/> class.
        /// </summary>
        public AuthorizationView()
        {
            this.InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await App.PushModalAsync(new LoginPage()).ConfigureAwait(false);
        }
    }
}
