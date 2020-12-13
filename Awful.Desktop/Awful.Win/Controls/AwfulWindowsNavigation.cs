// <copyright file="AwfulWindowsNavigation.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.Webview.Entities.Themes;
using Awful.Win.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Xamarin.Essentials;

namespace Awful.Win.Controls
{
    /// <summary>
    /// Awful Windows Navigation.
    /// </summary>
    public class AwfulWindowsNavigation : IAwfulNavigation
    {
        private IPlatformProperties platformProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulWindowsNavigation"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        /// <param name="platform">Awful Platform.</param>
        public AwfulWindowsNavigation(IAwfulContext context, IPlatformProperties platform)
        {
            this.SettingsAction = new SettingsAction(context);
            this.platformProperties = platform;
        }

        /// <summary>
        /// Gets the Settings Actions. Used to handle settings.
        /// </summary>
        public SettingsAction SettingsAction { get; internal set; }

        /// <inheritdoc/>
        public Task DisplayAlertAsync(string title, string message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
                {
                    ContentDialog alertDialog = new ContentDialog()
                    {
                        Title = title,
                        Content = message,
                        CloseButtonText = "Ok",
                    };

                    await alertDialog.ShowAsync();
                });
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<string> DisplayPromptAsync(string title, string message, string placeholder = "Text", string initialValue = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task LogoutAsync(IAwfulContext context, IPlatformProperties properties)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task PopModalAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task PushDetailPageAsync(object page)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task PushModalAsync(object page)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task PushPageAsync(object page)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RefreshForumPageAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RefreshPostPageAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void SetMainAppPage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var rootFrame = Window.Current?.Content as Frame;
                if (rootFrame != null)
                {
                    rootFrame.Navigate(typeof(MainPage));
                }
            });
        }

        /// <inheritdoc/>
        public Task SetMainAppPageAsync()
        {
            this.SetMainAppPage();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void SetTheme(DeviceColorTheme theme)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SetupThemeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
