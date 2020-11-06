// <copyright file="SettingsPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Settings Page.
    /// </summary>
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<SettingsViewModel>();
        }
    }
}
