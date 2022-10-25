// <copyright file="ThreadActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.Threads;
using Awful.Entities.Web;
using Awful.Managers;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Actions.
    /// </summary>
    public class ThreadActions
    {
        private IDatabaseContext context;
        private ThreadManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public ThreadActions(AwfulClient client, IDatabaseContext context)
        {
            manager = new ThreadManager(client);
            this.context = context;
        }

        /// <summary>
        /// Mark a thread as 'Unread'.
        /// </summary>
        /// <param name="threadId">The Thread Id.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Task Result.</returns>
        public async Task<Result> MarkThreadUnreadAsync(long threadId, CancellationToken token = default)
        {
            return await manager.MarkThreadUnreadAsync(threadId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Marks post as last read.
        /// </summary>
        /// <param name="threadId">The thread id.</param>
        /// <param name="index">The post number index.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> MarkPostAsLastReadAsAsync(long threadId, long index, CancellationToken token = default)
        {
            return await manager.MarkPostAsLastReadAsAsync(threadId, index, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new thread preview.
        /// </summary>
        /// <param name="newThreadEntity">New Thread Entity.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A post.</returns>
        public async Task<string> CreateNewThreadPreviewAsync(NewThread newThreadEntity, CancellationToken token = default)
        {
            return await manager.CreateNewThreadPreviewAsync(newThreadEntity, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new SA Thread.
        /// </summary>
        /// <param name="newThreadEntity">A New Thread Entity.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A result.</returns>
        public async Task<Result> PostNewThreadAsync(NewThread newThreadEntity, CancellationToken token = default)
        {
            return await manager.CreateNewThreadAsync(newThreadEntity, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a new thread cookies. Needed to make a new thread.
        /// </summary>
        /// <param name="forumId">The forum id for where to make the thread.</param>
        /// <param name="token">The CancellationToken.</param>
        /// <returns>A NewThread.</returns>
        public async Task<(string formKey, string formCookie)> CreateNewThreadAsync(int forumId, CancellationToken token = default)
        {
            return await manager.GetThreadCookiesAsync(forumId, token).ConfigureAwait(false);
        }
    }
}
