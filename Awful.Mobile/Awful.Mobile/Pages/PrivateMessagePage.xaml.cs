// <copyright file="PrivateMessagePage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Private Message Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivateMessagePage : BasePage
    {
        private PrivateMessagePageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagePage"/> class.
        /// </summary>
        /// <param name="pm">Private Message.</param>
        public PrivateMessagePage(AwfulPM pm)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<PrivateMessagePageViewModel>();
            this.vm.WebView = this.AwfulWebView;
            this.vm.LoadPM(pm);
        }
    }
}
