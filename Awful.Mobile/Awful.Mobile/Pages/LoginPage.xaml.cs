// <copyright file="LoginPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Login page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<LoginPageViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var navigation = App.Container.Resolve<IAwfulNavigation>();
            var database = App.Container.Resolve<IAwfulContext>();
            navigation.SetTheme(database.GetAppSettings());
        }
    }
}
