// <copyright file="LoginPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Autofac;
using Awful.UI.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Awful.Win.Pages
{
    /// <summary>
    /// Login page.
    /// </summary>
    public sealed partial class LoginPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Container.Resolve<LoginPageViewModel>();
        }

        /// <summary>
        /// Gets the View Model.
        /// </summary>
        public LoginPageViewModel ViewModel => this.DataContext as LoginPageViewModel;
    }
}
