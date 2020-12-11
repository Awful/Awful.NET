// <copyright file="ThreadActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Smilies;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
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
    /// Thread Actions.
    /// </summary>
    public class ThreadActions
    {
        private IAwfulContext context;
        private ThreadManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public ThreadActions(AwfulClient client, IAwfulContext context)
        {
            this.manager = new ThreadManager(client);
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
            return await this.manager.MarkThreadUnreadAsync(threadId, token).ConfigureAwait(false);
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
            return await this.manager.MarkPostAsLastReadAsAsync(threadId, index, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new thread preview.
        /// </summary>
        /// <param name="newThreadEntity">New Thread Entity.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A post.</returns>
        public async Task<Post> CreateNewThreadPreviewAsync(NewThread newThreadEntity, CancellationToken token = default)
        {
            return await this.manager.CreateNewThreadPreviewAsync(newThreadEntity, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new SA Thread.
        /// </summary>
        /// <param name="newThreadEntity">A New Thread Entity.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A result.</returns>
        public async Task<Result> PostNewThreadAsync(NewThread newThreadEntity, CancellationToken token = default)
        {
            return await this.manager.CreateNewThreadAsync(newThreadEntity, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a new thread cookies. Needed to make a new thread.
        /// </summary>
        /// <param name="forumId">The forum id for where to make the thread.</param>
        /// <param name="token">The CancellationToken.</param>
        /// <returns>A NewThread.</returns>
        public async Task<NewThread> CreateNewThreadAsync(int forumId, CancellationToken token = default)
        {
            return await this.manager.GetThreadCookiesAsync(forumId, token).ConfigureAwait(false);
        }
    }
}
