// <copyright file="Post.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.Posts
{
    /// <summary>
    /// Something Awful Post.
    /// </summary>
    public class Post : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class.
        /// </summary>
        /// <param name="postId">Id.</param>
        /// <param name="postHtml">Html.</param>
        /// <param name="postDate">Post Date.</param>
        /// <param name="user">User.</param>
        /// <param name="postIndex">Post Index.</param>
        /// <param name="hasSeen">Has Seen Post.</param>
        /// <param name="isIgnored">Is Post Ignored.</param>
        public Post(long postId, string postHtml, DateTime postDate, User user, long postIndex = 0, bool hasSeen = false, bool isIgnored = false)
        {
            this.PostId = postId;
            this.PostHtml = postHtml;
            this.PostDate = postDate;
            this.PostIndex = postIndex;
            this.HasSeen = hasSeen;
            this.IsIgnored = isIgnored;
            this.User = user;
        }

        /// <summary>
        /// Gets the user post.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Gets the post date.
        /// </summary>
        public DateTime PostDate { get; }

        /// <summary>
        /// Gets the post html.
        /// </summary>
        public string PostHtml { get; }

        /// <summary>
        /// Gets the post id.
        /// </summary>
        public long PostId { get; }

        /// <summary>
        /// Gets the post index.
        /// </summary>
        public long PostIndex { get; }

        /// <summary>
        /// Gets a value indicating whether the post has been seen.
        /// </summary>
        public bool HasSeen { get; }

        /// <summary>
        /// Gets a value indicating whether the post is ignored.
        /// </summary>
        public bool IsIgnored { get; }
    }
}
