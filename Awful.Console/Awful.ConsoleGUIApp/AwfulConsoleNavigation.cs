// <copyright file="AwfulConsoleNavigation.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.Webview.Entities.Themes;

namespace Awful.ConsoleGUIApp
{
    /// <summary>
    /// Awful Console Navigation.
    /// </summary>
    public class AwfulConsoleNavigation : IAwfulNavigation
    {
        /// <inheritdoc/>
        public Task DisplayAlertAsync(string title, string message)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SetMainAppPageAsync()
        {
            throw new NotImplementedException();
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
