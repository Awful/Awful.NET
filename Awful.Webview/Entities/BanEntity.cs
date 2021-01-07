// <copyright file="BanEntity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.Bans;
using Awful.Core.Entities.Users;

namespace Awful.Webview.Entities
{
    /// <summary>
    /// Ban template entity.
    /// </summary>
    public class BanEntity : TemplateEntity
    {
        /// <summary>
        /// Gets or sets the BanPage Entry.
        /// </summary>
        public BanPage Entry { get; set; }
    }
}
