﻿// <copyright file="SAclopediaEntryPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Core.Entities.SAclopedia;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// SAclopedia Entry.
    /// </summary>
    public partial class SAclopediaEntryPage : BasePage
    {
        private SAclopediaEntryPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryPage"/> class.
        /// </summary>
        public SAclopediaEntryPage(SAclopediaEntryItem entry)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<SAclopediaEntryPageViewModel>();
            this.vm.WebView = this.AwfulWebView;
            this.vm.LoadEntry(entry);
        }
    }
}
