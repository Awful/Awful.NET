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
using Awful.UI.Actions;
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
        private SettingsAction settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulShell"/> class.
        /// </summary>
        public AwfulShell()
        {
            this.InitializeComponent();
            this.settings = new SettingsAction(this.context);
            Routing.RegisterRoute("saclopediaentrypage", typeof(SAclopediaEntryPage));
            Routing.RegisterRoute("signinpage", typeof(SigninPage));
            Device.BeginInvokeOnMainThread(async () =>
            {
                var settings = this.context.SettingOptionsItems.FirstOrDefault();
                if (settings != null)
                {
                    this.settings.SetAppTheme(settings.DeviceColorTheme);
                }

                //if (this.context.GetDefaultUser() == null)
                //{
                //    await Shell.Current.GoToAsync("//SigninPage").ConfigureAwait(false);
                //}
            });

            this.HeaderImage.Source = ImageSource.FromResource("Awful.Mobile.ThreadTags.Mazui.png");
        }
    }
}