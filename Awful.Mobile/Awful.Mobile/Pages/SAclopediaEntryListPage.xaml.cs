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

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// SAclopedia Entry List.
    /// </summary>
    public partial class SAclopediaEntryListPage : BasePage, IAwfulSearchPage
    {
        private SAclopediaEntryListPageViewModel vm;

        public SAclopediaEntryListPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<SAclopediaEntryListPageViewModel>();
        }

        /// <inheritdoc/>
        public event EventHandler<string> SearchBarTextChanged;

        /// <inheritdoc/>
        public void OnSearchBarTextChanged(in string text) => this.vm.FilterList(text);
    }
}
