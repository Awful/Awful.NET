// <copyright file="SAclopediaPost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Post.
    /// </summary>
    public class SAclopediaPost
    {
        /// <summary>
        /// Gets or sets the id of the user who made the post.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the inner contents of the post.
        /// </summary>
        public string? PostHtml { get; set; }

        /// <summary>
        /// Gets or sets the date the post was posted.
        /// </summary>
        public DateTime PostedDate { get; set; }
    }
}
