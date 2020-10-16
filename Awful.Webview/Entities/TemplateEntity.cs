// <copyright file="TemplateEntity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Webview.Entities
{
    /// <summary>
    /// Default template entity.
    /// Used as a base for creating a template.
    /// </summary>
    public class TemplateEntity
    {
        /// <summary>
        /// Gets or sets the device theme.
        /// </summary>
        public string Theme { get; set; } = "aurora";

        /// <summary>
        /// Gets or sets the color theme for the template.
        /// </summary>
        public string DeviceColorTheme { get; set; } = "theme";

        /// <summary>
        /// Gets or sets the CSS used for the template.
        /// Rendered in order.
        /// </summary>
        public List<string> CSS { get; set; }

        /// <summary>
        /// Gets or sets the Javascript used for the template.
        /// Rendered in order.
        /// </summary>
        public List<string> JS { get; set; }
    }
}
