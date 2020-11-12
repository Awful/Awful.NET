// <copyright file="LepersPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Lepers Page.
    /// </summary>
    public partial class LepersPage : AuthorizationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LepersPage"/> class.
        /// </summary>
        public LepersPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<LepersViewModel>();
        }
    }
}
