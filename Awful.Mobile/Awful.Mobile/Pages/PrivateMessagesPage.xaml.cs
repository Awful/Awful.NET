// <copyright file="PrivateMessagesPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Private Messages Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivateMessagesPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagesPage"/> class.
        /// </summary>
        public PrivateMessagesPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<MobilePrivateMessagesPageViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            this.ThreadListCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}