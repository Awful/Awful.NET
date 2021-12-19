// <copyright file="ThreadPost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Thread Post.
    /// </summary>
    public class ThreadPost : SAItem
    {
        /// <summary>
        /// Gets a value indicating whether the thread is archived.
        /// </summary>
        public bool IsArchived { get; internal set; }

        /// <summary>
        /// Gets the logged in username.
        /// </summary>
        public string? LoggedInUserName { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the user in logged in.
        /// </summary>
        public bool IsLoggedIn { get; internal set; }

        /// <summary>
        /// Gets the post to scroll to.
        /// </summary>
        public int ScrollToPost { get; internal set; }

        /// <summary>
        /// Gets the post to scroll string.
        /// </summary>
        public string? ScrollToPostString { get; internal set; }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage { get; internal set; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this is the last page.
        /// </summary>
        public bool LastPage => this.CurrentPage >= this.TotalPages;

        /// <summary>
        /// Gets the name of the thread.
        /// </summary>
        public string? Name { get; internal set; }

        /// <summary>
        /// Gets the thread id.
        /// </summary>
        public int ThreadId { get; internal set; }

        /// <summary>
        /// Gets the forum id.
        /// </summary>
        public int ForumId { get; internal set; }

        /// <summary>
        /// Gets the list of threads.
        /// </summary>
        public List<Post> Posts { get; internal set; } = new List<Post>();

        /// <summary>
        /// Gets the parent forum name.
        /// </summary>
        public string? ParentForumName { get; internal set; }

        /// <summary>
        /// Gets the parent forum id.
        /// </summary>
        public int ParentForumId { get; internal set; }

        /// <summary>
        /// Gets the forum name.
        /// </summary>
        public string? ForumName { get; internal set; }
    }
}
