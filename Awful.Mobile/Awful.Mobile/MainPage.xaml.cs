// <copyright file="MainPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Awful.Mobile
{
    /// <summary>
    /// Main Page. Used for Navigation.
    /// </summary>
    public partial class MainPage : FlyoutPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += this.MainPage_SizeChanged;
        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width < 400)
            {
                this.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
            }
            else
            {
                this.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Split;
            }
        }

        protected override void OnAppearing()
        {
        }
    }
}
