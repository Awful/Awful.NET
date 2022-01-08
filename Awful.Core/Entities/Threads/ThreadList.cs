// <copyright file="ThreadList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Forums;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Thread List.
    /// </summary>
    public class ThreadList : SAItem
    {
        public ThreadList(int id, int currentPage, int totalPages, Forum forum, IEnumerable<Thread> threads)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.Forum = forum;
            this.Threads = threads.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets the forum.
        /// </summary>
        public Forum Forum { get; }

        /// <summary>
        /// Gets the threads.
        /// </summary>
        public IReadOnlyList<Thread> Threads { get; }
    }
}
