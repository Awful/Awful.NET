// <copyright file="SAclopediaPost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Post.
    /// </summary>
    public class SAclopediaPost
    {
        public SAclopediaPost(int userId, string username, string postHtml, DateTime postedDate)
        {
            this.PostHtml = postHtml;
            this.Username = username;
            this.PostedDate = postedDate;
            this.UserId = userId;
        }

        /// <summary>
        /// Gets the id of the user who made the post.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string? Username { get; }

        /// <summary>
        /// Gets the inner contents of the post.
        /// </summary>
        public string? PostHtml { get; }

        /// <summary>
        /// Gets the date the post was posted.
        /// </summary>
        public DateTime PostedDate { get; }
    }
}
