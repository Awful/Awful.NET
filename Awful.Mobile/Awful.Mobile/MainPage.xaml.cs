// <copyright file="MainPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Mobile.Pages;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile
{
    /// <summary>
    /// Main Page. Used for Navigation on large devices.
    /// Small devices use <see cref="MainTabbedPage"/> alone.
    /// </summary>
    public partial class MainPage : FlyoutPage
    {
        private MobileAwfulViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.vm = App.Container.Resolve<MobileAwfulViewModel>();
            this.vm.SetupThemeAsync().ConfigureAwait(false);
        }
    }
}
