// <copyright file="ThreadListActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.Threads;
using Awful.Managers;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread List Actions.
    /// </summary>
    public class ThreadListActions
    {
        private IDatabaseContext context;
        private ThreadListManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadListActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public ThreadListActions(AwfulClient client, IDatabaseContext context)
        {
            manager = new ThreadListManager(client);
            this.context = context;
        }

        /// <summary>
        /// Gets the list of threads in a given Forum.
        /// </summary>
        /// <param name="forumId">The Forum Id.</param>
        /// <param name="page">The page of the forum to get.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A ThreadList.</returns>
        public async Task<ThreadList> GetForumThreadListAsync(int forumId, int page, CancellationToken token = default)
        {
            return await manager.GetForumThreadListAsync(forumId, page, token).ConfigureAwait(false);
        }
    }
}
