// <copyright file="DefaultOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Webview.Entities.Themes
{
    /// <summary>
    /// Default options for themes.
    /// </summary>
    public class DefaultOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether we're using dark mode.
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we're using Oled mode.
        /// </summary>
        public bool IsOledMode { get; set; }

        /// <summary>
        /// Gets or sets the default forum theme id.
        /// </summary>
        public int ForumThemeId { get; set; }
    }
}
