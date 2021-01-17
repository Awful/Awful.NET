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
        /// Gets a value indicating whether the current platform is running a system level dark theme.
        /// </summary>
        bool IsDarkTheme { get; }

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
