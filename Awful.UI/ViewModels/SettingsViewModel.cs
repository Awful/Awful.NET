// <copyright file="SettingsViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.Webview.Entities.Themes;
using Microsoft.EntityFrameworkCore;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Settings View Model.
    /// </summary>
    public class SettingsViewModel : AwfulViewModel
    {
        private SettingsAction settingActions;
        private SettingOptions settings;
        private DeviceColorTheme deviceColorTheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public SettingsViewModel(AwfulContext context)
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
