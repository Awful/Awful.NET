// <copyright file="BookmarkListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Views
{
    public partial class BookmarkListPage : ContentPage
    {
        public BookmarkListPage()
        {
            InitializeComponent();
            this.BindingContext = App.Container.Resolve<BookmarksViewModel>();
        }
    }
}
