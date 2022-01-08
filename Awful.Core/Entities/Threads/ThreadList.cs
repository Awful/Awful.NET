// <copyright file="ThreadList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Thread List.
    /// </summary>
    public class ThreadList : SAItem
    {
        public ThreadList(int id, int forumId, string forumName, int currentPage, int totalPages, IEnumerable<Thread> threads, int parentForumId = 0, string? parentForumName = null)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.ParentForumName = parentForumName ?? string.Empty;
            this.ForumId = forumId;
            this.ForumName = forumName;
            this.Threads = threads.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the parent forum name.
        /// </summary>
        public string ParentForumName { get; set; }

        /// <summary>
        /// Gets or sets the parent forum id.
        /// </summary>
        public int ParentForumId { get; set; }

        /// <summary>
        /// Gets or sets the forum name.
        /// </summary>
        public string ForumName { get; set; }

        /// <summary>
        /// Gets or sets the forum id.
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// Gets the threads.
        /// </summary>
        public IReadOnlyList<Thread> Threads { get; }
    }
}
