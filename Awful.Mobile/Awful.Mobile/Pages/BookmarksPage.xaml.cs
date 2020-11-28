// <copyright file="BookmarksPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Bookmarks Page.
    /// </summary>
    public partial class BookmarksPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksPage"/> class.
        /// </summary>
        public BookmarksPage()
        {
            InitializeComponent();
            this.BindingContext = App.Container.Resolve<BookmarksPageViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            this.ThreadListCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}
