// <copyright file="ProfileEntity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.Users;

namespace Awful.Webview.Entities
{
    /// <summary>
    /// Profile template entity.
    /// </summary>
    public class ProfileEntity : TemplateEntity
    {
        /// <summary>
        /// Gets or sets the Profile Entry.
        /// </summary>
        public Awful.Core.Entities.JSON.User Entry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the theme is dark.
        /// </summary>
        public bool IsDark { get; set; }
    }
}
