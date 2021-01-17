// <copyright file="SAclopediaEntryListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// SAclopedia Entry List.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SAclopediaEntryListPage : BasePage
    {
        private MobileSAclopediaEntryListPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryListPage"/> class.
        /// </summary>
        public SAclopediaEntryListPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<MobileSAclopediaEntryListPageViewModel>();
        }
    }
}
