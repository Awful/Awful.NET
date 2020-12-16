// <copyright file="AwfulConsolePlatform.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Webview.Entities.Themes;

namespace Awful.ConsoleGUIApp
{
    /// <summary>
    /// Awful Console Platform.
    /// </summary>
    public class AwfulConsolePlatform : IPlatformProperties
    {
        /// <summary>
        /// Gets the cookie path.
        /// </summary>
        public string CookiePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "awful.cookie");

        /// <summary>
        /// Gets the Database Path.
        /// </summary>
        public string DatabasePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "awful.db");

        /// <inheritdoc/>
        public DeviceColorTheme GetTheme()
        {
            return DeviceColorTheme.Dark;
        }
    }
}
