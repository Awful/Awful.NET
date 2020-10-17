// <copyright file="SmileCategory.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Smilies
{
    /// <summary>
    /// Something Awful Smile Category.
    /// </summary>
    public class SmileCategory
    {
        /// <summary>
        /// Gets the list of smilies.
        /// </summary>
        public List<Smile> SmileList { get; } = new List<Smile>();

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name { get; set; }
    }
}
