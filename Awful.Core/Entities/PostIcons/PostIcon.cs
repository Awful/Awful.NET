// <copyright file="PostIcon.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.PostIcons
{
    /// <summary>
    /// Something Awful Post Icon.
    /// </summary>
    public class PostIcon
    {
        /// <summary>
        /// Gets or sets the id of the post icon.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the image icon endpoint.
        /// </summary>
        public string ImageEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the title of the post icon.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the image location of the icon.
        /// </summary>
        public string ImageLocation { get; internal set; }
    }
}
