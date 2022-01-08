// <copyright file="PrivateMessagePost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Messages
{
    /// <summary>
    /// Private Message Post.
    /// </summary>
    public class PrivateMessagePost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagePost"/> class.
        /// </summary>
        /// <param name="post">Private Message Post.</param>
        /// <param name="pm">Private Message.</param>
        public PrivateMessagePost(Post post, PrivateMessage pm)
        {
            this.Post = post;
            this.PM = pm;
        }

        /// <summary>
        /// Gets the post.
        /// </summary>
        public Post Post { get; }

        /// <summary>
        /// Gets the post.
        /// </summary>
        public PrivateMessage PM { get; }
    }
}
