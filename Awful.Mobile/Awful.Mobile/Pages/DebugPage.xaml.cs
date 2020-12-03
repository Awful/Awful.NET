// <copyright file="DebugPage.xaml.cs" company="Drastic Actions">
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
    /// Debug Page. Used for testing out views.
    /// </summary>
    public partial class DebugPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugPage"/> class.
        /// </summary>
        public DebugPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<DebugPageViewModel>();
        }
    }
}
