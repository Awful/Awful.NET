// <copyright file="Post.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Posts
{
    /// <summary>
    /// Something Awful Post.
    /// </summary>
    public class Post : SAItem
    {
        /// <summary>
        /// Gets or sets the user post.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the post date.
        /// </summary>
        public string PostDate { get; set; }

        /// <summary>
        /// Gets or sets the post html.
        /// </summary>
        public string PostHtml { get; set; }

        /// <summary>
        /// Gets or sets the post id.
        /// </summary>
        public long PostId { get; set; }

        /// <summary>
        /// Gets or sets the post index.
        /// </summary>
        public long PostIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the post has been seen.
        /// </summary>
        public bool HasSeen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the post is ignored.
        /// </summary>
        public bool IsIgnored { get; set; }
    }
}
