﻿// <copyright file="DebugPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

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
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await App.SetDetailPageAsync(new DefaultPage());
        }
    }
}