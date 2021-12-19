// <copyright file="SAclopediaEntry.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Entry.
    /// </summary>
    public class SAclopediaEntry : SAItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the list of SAclopedia Posts.
        /// </summary>
        public List<SAclopediaPost> Posts { get; } = new List<SAclopediaPost>();
    }
}
