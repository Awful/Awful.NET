// <copyright file="WindowsSettingsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;

namespace Awful.Win.ViewModels
{
    /// <summary>
    /// Settings Page View Model.
    /// </summary>
    public class WindowsSettingsPageViewModel : SettingsPageViewModel
    {
        private bool isDark;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsSettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="platformProperties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public WindowsSettingsPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IPlatformProperties platformProperties, IAwfulContext context) : base(navigation, error, platformProperties, context)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view is dark.
        /// </summary>
        public bool IsDark
        {
            get
            {
                return this.isDark;
            }

            set
            {
                if (value == false)
                {
                    this.DeviceColorTheme = Webview.Entities.Themes.DeviceColorTheme.Light;
                }
                else
                {
                    this.DeviceColorTheme = Webview.Entities.Themes.DeviceColorTheme.Dark;
                }

                this.SetProperty(ref this.isDark, value);
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            this.IsDark = this.DeviceColorTheme == Webview.Entities.Themes.DeviceColorTheme.Dark;
        }
    }
}
