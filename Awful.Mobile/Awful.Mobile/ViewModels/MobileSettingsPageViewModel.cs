// <copyright file="MobileSettingsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview.Entities.Themes;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Settings Page View Model.
    /// </summary>
    public class MobileSettingsPageViewModel : MobileAwfulViewModel
    {
        private SettingsAction settingActions;
        private SettingOptions settings;
        private DeviceColorTheme deviceColorTheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileSettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public MobileSettingsPageViewModel(AwfulContext context)
            : base(context)
        {
            this.settings = new SettingOptions();
            this.settingActions = new SettingsAction(context);
        }

        /// <summary>
        /// Gets the theme names.
        /// </summary>
        public List<string> ThemeNames
        {
            get
            {
                return Enum.GetNames(typeof(DeviceColorTheme)).Select(b => b).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the device color theme.
        /// </summary>
        public DeviceColorTheme DeviceColorTheme
        {
            get
            {
                return this.deviceColorTheme;
            }

            set
            {
                this.deviceColorTheme = value;
                if (value != this.settings.DeviceColorTheme)
                {
                    this.settings.DeviceColorTheme = value;
                    Task.Run(async () =>
                    {
                        this.settingActions.SetAppTheme(value);
                        await this.SaveSettingsAsync().ConfigureAwait(false);
                    });
                }

                this.OnPropertyChanged(nameof(this.DeviceColorTheme));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable background tasks.
        /// </summary>
        public bool EnableBackgroundTasks
        {
            get
            {
                return this.settings.EnableBackgroundTasks;
            }

            set
            {
                this.settings.EnableBackgroundTasks = value;
                this.OnPropertyChanged(nameof(this.EnableBackgroundTasks));
                Task.Run(async () => await this.SaveSettingsAsync().ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Gets the login page command.
        /// </summary>
        public AwfulAsyncCommand LoginPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (!this.IsSignedIn)
                    {
                        await MobileAwfulViewModel.PushModalAsync(new LoginPage()).ConfigureAwait(false);
                    }
                    else
                    {
                        bool answer = await Application.Current.MainPage.DisplayAlert("Log Out", "Are you sure you want to log out?", "Yep", "Nope").ConfigureAwait(false);
                        if (answer)
                        {
                            this.Context.ResetDatabase();
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                        }
                    }
                },
                    null,
                    this);
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            this.settings = await this.Context.SettingOptionsItems.FirstOrDefaultAsync().ConfigureAwait(false) ?? new SettingOptions();
            this.DeviceColorTheme = this.settings.DeviceColorTheme;
        }

        private async Task SaveSettingsAsync()
        {
            await this.settingActions.SaveSettingOptionsAsync(this.settings).ConfigureAwait(false);
        }
    }
}
