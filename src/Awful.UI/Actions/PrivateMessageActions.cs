// <copyright file="PrivateMessageActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities;
using Awful.Entities.Messages;
using Awful.Entities.Posts;
using Awful.Entities.Threads;
using Awful.Managers;
using Awful.Themes;
using Awful.UI.Entities;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Private Message Actions.
    /// </summary>
    public class PrivateMessageActions
    {
        private IDatabaseContext context;
        private PrivateMessageManager manager;
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public PrivateMessageActions(AwfulClient client, IDatabaseContext context, ITemplateHandler templates)
        {
            manager = new PrivateMessageManager(client);
            this.context = context;
            this.templates = templates;
        }

        /// <summary>
        /// Get all PMs for given user.
        /// </summary>
        /// <returns>List of Thread Bookmarks.</returns>
        /// <param name="forceRefresh">Force Refresh.</param>
        /// <param name="token">Cancelation Token.</param>
        public async Task<List<AwfulPM>> GetAllPrivateMessagesAsync(bool forceRefresh = false, CancellationToken token = default)
        {
            var pms = await context.GetAllPrivateMessagesAsync().ConfigureAwait(false);
            if (!pms.Any() || forceRefresh)
            {
                var threads = await manager.GetAllPrivateMessageListAsync(token).ConfigureAwait(false);
                pms = await context.AddAllPrivateMessagesAsync(threads.PrivateMessages).ConfigureAwait(false);
            }

            return pms.OrderBy(n => n.SortOrder).ToList();
        }

        /// <summary>
        /// Renders PM as HTML string.
        /// </summary>
        /// <param name="entry">The Post.</param>
        /// <param name="defaultOptions">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderPrivateMessageView(Post entry, DefaultOptions defaultOptions)
        {
            var threadPost = new ThreadPost(entry);
            return templates.RenderThreadPostView(threadPost, defaultOptions);
        }

        /// <summary>
        /// Get Private Message.
        /// </summary>
        /// <param name="id">PM id.</param>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>Post</returns>
        public async Task<Post> GetPrivateMessageAsync(int id, CancellationToken token = default)
        {
            return await manager.GetPrivateMessageAsync(id, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Send Private Message.
        /// </summary>
        /// <param name="newPrivateMessageEntity">New PM.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<SAItem> SendPrivateMessageAsync(NewPrivateMessage newPrivateMessageEntity, CancellationToken token = default)
        {
            return await manager.SendPrivateMessageAsync(newPrivateMessageEntity, token).ConfigureAwait(false);
        }
    }
}
