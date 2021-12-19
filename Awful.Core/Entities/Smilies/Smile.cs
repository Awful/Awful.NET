// <copyright file="Smile.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Smilies
{
    /// <summary>
    /// Something Awful Smile (Emoji).
    /// </summary>
    public class Smile
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the image endpoint.
        /// </summary>
        public string? ImageEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the image location.
        /// </summary>
        public string? ImageLocation { get; set; }
    }
}
