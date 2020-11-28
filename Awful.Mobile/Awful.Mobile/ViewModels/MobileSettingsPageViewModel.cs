// <copyright file="MobileSettingsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Settings Page View Model.
    /// </summary>
    public class MobileSettingsPageViewModel : SettingsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileSettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileSettingsPageViewModel(IPlatformProperties properties, AwfulContext context)
            : base(properties, context)
        {
        }

        /// <summary>
        /// Gets the login page command.
        /// </summary>
        public Command LoginPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!this.IsSignedIn)
                    {
                        await App.PushModalAsync(new LoginPage()).ConfigureAwait(false);
                    }
                    else
                    {
                        bool answer = await Application.Current.MainPage.DisplayAlert("Log Out", "Are you sure you want to log out?", "Yep", "Nope").ConfigureAwait(false);
                        if (answer)
                        {
                            this.Context.ResetDatabase();
                            Device.BeginInvokeOnMainThread(() => App.SetMainAppPage());
                        }
                    }
                });
            }
        }
    }
}
