// <copyright file="DefaultPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Default Page. Used to show logo when no content is in detail.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPage"/> class.
        /// </summary>
        public DefaultPage()
        {
            this.InitializeComponent();
        }
    }
}
