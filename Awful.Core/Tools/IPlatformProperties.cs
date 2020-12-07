// <copyright file="IPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Webview.Entities.Themes;

namespace Awful.Core.Tools
{
    /// <summary>
    /// Platform Properties.
    /// </summary>
    public interface IPlatformProperties
    {
        /// <summary>
        /// Gets the device theme.
        /// </summary>
        /// <returns><see cref="DeviceColorTheme"/>.</returns>
        DeviceColorTheme GetTheme();

        /// <summary>
        /// Gets the path to where cookies are stored.
        /// </summary>
        string CookiePath { get; }

        /// <summary>
        /// Gets the path to where the database is stored.
        /// </summary>
        string DatabasePath { get; }
    }
}
