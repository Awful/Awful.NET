// <copyright file="AwfulShell.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile
{
    /// <summary>
    /// Awful Shell.
    /// </summary>
    public partial class AwfulShell : Xamarin.Forms.Shell
    {
        private AwfulContext context = App.Container.Resolve<AwfulContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulShell"/> class.
        /// </summary>
        public AwfulShell()
        {
            this.InitializeComponent();
            Routing.RegisterRoute("saclopediaentrypage", typeof(SAclopediaEntryPage));
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (this.context.GetDefaultUser() == null)
                {
                    await Shell.Current.GoToAsync("//SigninPage").ConfigureAwait(false);
                }
            });
        }
    }
}