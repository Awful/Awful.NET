// <copyright file="ThreadPostEntity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Webview.Entities
{
    /// <summary>
    /// Thread Post Entity.
    /// </summary>
    public class ThreadPostEntity : TemplateEntity
    {
        /// <summary>
        /// Gets or sets the Thread Post Entry.
        /// </summary>
        public Awful.Core.Entities.Threads.ThreadPost Entry { get; set; }
    }
}
