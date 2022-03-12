// <copyright file="ThreadPost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Forums;
using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Thread Post.
    /// </summary>
    public class ThreadPost : SAItem
    {
        public ThreadPost()
        {
        }

        public ThreadPost(Post post)
        {
            this.Posts = new List<Post>() { post };
        }

        public ThreadPost(int id, string name, Forum forum, IEnumerable<Post> posts, int currentPage = 0, int totalPages = 0, int scrollToPost = 0, string? scrollToPostString = null, string? loggedInUserName = null, bool isLoggedIn = false, bool isArchived = false)
        {
            this.ThreadId = id;
            this.Name = name;
            this.IsArchived = isArchived;
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.LoggedInUserName = loggedInUserName ?? string.Empty;
            this.IsLoggedIn = isLoggedIn;
            this.ScrollToPost = scrollToPost;
            this.Forum = forum;
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
        /// This is based on the forum page, which represents whatever options
        /// are set by the user on SA. This may be different for different users.
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
        /// Gets the list of threads.
        /// </summary>
        public IReadOnlyList<Post> Posts { get; } = new List<Post>(); 

        /// <summary>
        /// Gets the forum.
        /// </summary>
        public Forum? Forum { get; internal set; }
    }
}
