// <copyright file="ThreadPost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Thread Post.
    /// </summary>
    public class ThreadPost : SAItem
    {
        public ThreadPost(int id, string name, int forumId, string forumName, IEnumerable<Post> posts, int parentForumId = 0, string? parentForumName = null, int currentPage = 0, int totalPages = 0, int scrollToPost = 0, string? scrollToPostString = null, string? loggedInUserName = null, bool isLoggedIn = false, bool isArchived = false)
        {
            this.ThreadId = id;
            this.Name = name;
            this.ForumId = forumId;
            this.ForumName = forumName;
            this.IsArchived = isArchived;
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.LoggedInUserName = loggedInUserName ?? string.Empty;
            this.IsLoggedIn = isLoggedIn;
            this.ScrollToPost = scrollToPost;
            this.ParentForumId = parentForumId;
            this.ParentForumName = parentForumName ?? string.Empty;
            this.ScrollToPostString = scrollToPostString ?? string.Empty;
            this.Posts = posts.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the thread id.
        /// </summary>
        public int ThreadId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the thread is archived.
        /// </summary>
        public bool IsArchived { get; internal set; }

        /// <summary>
        /// Gets the logged in username.
        /// </summary>
        public string LoggedInUserName { get; internal set; }

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
        public string ScrollToPostString { get; internal set; }

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
        /// Gets the forum id.
        /// </summary>
        public int ForumId { get; internal set; }

        /// <summary>
        /// Gets the list of threads.
        /// </summary>
        public IReadOnlyList<Post> Posts { get; }

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
