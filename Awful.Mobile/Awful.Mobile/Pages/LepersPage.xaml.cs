// <copyright file="LepersPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Lepers Page.
    /// </summary>
    public partial class LepersPage : BasePage
    {
        private LepersPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="LepersPage"/> class.
        /// </summary>
        public LepersPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<LepersPageViewModel>();
            this.vm.WebView = this.AwfulWebView;
        }
    }
}
