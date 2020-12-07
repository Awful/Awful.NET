// <copyright file="DeviceTheme.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Webview.Entities.Themes
{
    /// <summary>
    /// Device Theme.
    /// </summary>
    public enum DeviceTheme
    {
        /// <summary>
        /// Default device theme.
        /// </summary>
        Default,

        /// <summary>
        /// iOS device theme.
        /// </summary>
#pragma warning disable SA1300 // Element should begin with upper-case letter
        iOS,
#pragma warning restore SA1300 // Element should begin with upper-case letter

        /// <summary>
        /// Android device theme.
        /// </summary>
        Android,
    }
}
