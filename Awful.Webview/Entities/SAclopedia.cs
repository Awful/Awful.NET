// <copyright file="SAclopedia.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.SAclopedia;

namespace Awful.Webview.Entities
{
    /// <summary>
    /// SAclopedia Template Item.
    /// </summary>
    public class SAclopedia : TemplateEntity
    {
        /// <summary>
        /// Gets or sets the SAclopedia Entry.
        /// </summary>
        public SAclopediaEntry Entry { get; set; }
    }
}
