﻿// <copyright file="MainPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile
{
    /// <summary>
    /// Main Page. Used for Navigation on large devices.
    /// Small devices use <see cref="MainTabbedPage"/> alone.
    /// </summary>
    public partial class MainPage : FlyoutPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var navigation = App.Container.Resolve<IAwfulNavigation>();
            var database = App.Container.Resolve<IAwfulContext>();
            navigation.SetTheme(database.GetAppSettings());
        }
    }
}
