// <copyright file="MasterPage.xaml.cs" company="Drastic Actions">
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
    /// Master Tabbed Page.
    /// </summary>
    public partial class MasterPage : TabbedPage
    {
        private MobileAwfulViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPage"/> class.
        /// </summary>
        public MasterPage()
        {
            this.InitializeComponent();
            this.vm = App.Container.Resolve<MobileAwfulViewModel>();
            this.vm.SetupThemeAsync().ConfigureAwait(false);
        }
    }
}
