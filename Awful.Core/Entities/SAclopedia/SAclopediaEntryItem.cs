// <copyright file="SAclopediaEntryItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Entry Item.
    /// </summary>
    public class SAclopediaEntryItem
    {
        /// <summary>
        /// Gets or sets the id of the entry item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the entry item.
        /// </summary>
        public string Title { get; set; }
    }
}
