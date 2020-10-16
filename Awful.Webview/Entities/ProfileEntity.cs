// <copyright file="ProfileEntity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.Users;
using Awful.Webview.Entities.Themes;

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
    }
}
