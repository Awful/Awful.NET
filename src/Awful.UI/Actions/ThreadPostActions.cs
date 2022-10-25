// <copyright file="ThreadPostActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.Threads;
using Awful.Managers;
using Awful.Themes;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Post Actions.
    /// </summary>
    public class ThreadPostActions
    {
        private IDatabaseContext context;
        private ThreadPostManager manager;
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public ThreadPostActions(AwfulClient client, IDatabaseContext context, ITemplateHandler templates)
        {
            manager = new ThreadPostManager(client);
            this.context = context;
            this.templates = templates;
        }

        /// <summary>
        /// Gets a thread. Can be used with or without authentication, but depending on the thread it may be behind a paywall.
        /// This should be wrapped to check for <see cref="PaywallException"/>.
        /// </summary>
        /// <param name="threadId">A Thread Id.</param>
        /// <param name="pageNumber">The page number. Defaults to 1.</param>
        /// <param name="goToNewestPost">Goes to the newest page and post in a thread. Overrides pageNumber if set to True.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Thread.</returns>
        public async Task<ThreadPost> GetThreadPostsAsync(int threadId, int pageNumber = 1, bool goToNewestPost = false, CancellationToken token = default)
        {
            return await manager.GetThreadPostsAsync(threadId, pageNumber, goToNewestPost, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders Thread as HTML string.
        /// </summary>
        /// <param name="entry">The Thread Post.</param>
        /// <param name="defaultOptions">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderThreadPostView(ThreadPost entry, DefaultOptions defaultOptions)
        {
            return templates.RenderThreadPostView(entry, defaultOptions);
        }
    }
}
