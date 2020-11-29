// <copyright file="SAclopediaEntryListPage.xaml.cs" company="Drastic Actions">
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
    public partial class SAclopediaEntryListPage : BasePage
    {
        public SAclopediaEntryListPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<SAclopediaEntryListPageViewModel>();
        }
    }
}
