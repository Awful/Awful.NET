// <copyright file="ThreadPostActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Smilies;
using Awful.Core.Entities.Threads;
using Awful.Core.Managers;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Post Actions.
    /// </summary>
    public class ThreadPostActions
    {
        private IAwfulContext context;
        private ThreadPostManager manager;
        private TemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public ThreadPostActions(AwfulClient client, IAwfulContext context, TemplateHandler templates)
        {
            this.manager = new ThreadPostManager(client);
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
            return await this.manager.GetThreadPostsAsync(threadId, pageNumber, goToNewestPost, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders Thread as HTML string.
        /// </summary>
        /// <param name="entry">The Thread Post.</param>
        /// <param name="defaultOptions">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderThreadPostView(ThreadPost entry, DefaultOptions defaultOptions)
        {
            return this.templates.RenderThreadPostView(entry, defaultOptions);
        }
    }
}
