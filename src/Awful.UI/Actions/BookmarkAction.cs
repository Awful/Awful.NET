// <copyright file="BookmarkAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Managers;
using Awful.UI.Entities;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Bookmark Action.
    /// </summary>
    public class BookmarkAction
    {
        private IDatabaseContext context;
        private BookmarkManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkAction"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public BookmarkAction(AwfulClient client, IDatabaseContext context)
        {
            manager = new BookmarkManager(client);
            this.context = context;
        }

        /// <summary>
        /// Get all Bookmarks for given user.
        /// </summary>
        /// <returns>List of Thread Bookmarks.</returns>
        public async Task<List<AwfulThread>> GetAllBookmarksCachedAsync()
        {
            var bookmarks = await context.GetAllBookmarkThreadsAsync().ConfigureAwait(false);
            return bookmarks.OrderBy(n => n.SortOrder).ToList();
        }

        /// <summary>
        /// Get all Bookmarks for given user.
        /// </summary>
        /// <returns>List of Thread Bookmarks.</returns>
        /// <param name="forceRefresh">Force Refresh.</param>
        public async Task<List<AwfulThread>> GetAllBookmarksAsync(bool forceRefresh = false)
        {
            var bookmarks = await context.GetAllBookmarkThreadsAsync().ConfigureAwait(false);
            if (!bookmarks.Any() || forceRefresh)
            {
                var threads = await manager.GetAllBookmarksAsync().ConfigureAwait(false);
                bookmarks = await context.AddAllBookmarkThreadsAsync(threads.Threads).ConfigureAwait(false);
            }

            return bookmarks.OrderBy(n => n.SortOrder).ToList();
        }

        /// <summary>
        /// Enable or disable bookmark notifications for a given thread.
        /// </summary>
        /// <param name="thread">The AwfulThread.</param>
        /// <returns>The AwfulThread with the updated value.</returns>
        public async Task<AwfulThread> EnableDisableBookmarkNotificationAsync(AwfulThread thread)
        {
            return await context.EnableDisableBookmarkNotificationsEnableAsync(thread).ConfigureAwait(false);
        }

        /// <summary>
        /// Add Bookmark.
        /// </summary>
        /// <param name="threadId">Thread Id.</param>
        /// <returns>Result.</returns>
        public async Task<List<AwfulThread>> AddBookmarkAsync(int threadId)
        {
            var result = await manager.AddBookmarkAsync(threadId).ConfigureAwait(false);

            return await GetAllBookmarksAsync(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Add Bookmark.
        /// </summary>
        /// <param name="thread">Thread.</param>
        /// <returns>Result.</returns>
        public async Task<List<AwfulThread>> AddBookmarkAsync(Awful.Entities.Threads.Thread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            return await this.AddBookmarkAsync(thread.ThreadId).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove Bookmark.
        /// </summary>
        /// <param name="threadId">Thread Id.</param>
        /// <returns>Result.</returns>
        public async Task<List<AwfulThread>> RemoveBookmarkAsync(int threadId)
        {
            var result = await manager.RemoveBookmarkAsync(threadId).ConfigureAwait(false);
            return await GetAllBookmarksAsync(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove Bookmark.
        /// </summary>
        /// <param name="thread">Thread.</param>
        /// <returns>Result.</returns>
        public async Task<List<AwfulThread>> RemoveBookmarkAsync(Awful.Entities.Threads.Thread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            return await this.RemoveBookmarkAsync(thread.ThreadId).ConfigureAwait(false);
        }
    }
}
