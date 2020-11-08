// <copyright file="PrivateMessageActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.Web;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Private Message Actions.
    /// </summary>
    public class PrivateMessageActions
    {
        private AwfulContext context;
        private PrivateMessageManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public PrivateMessageActions(AwfulClient client, AwfulContext context)
        {
            this.manager = new PrivateMessageManager(client);
            this.context = context;
        }

        /// <summary>
        /// Get all PMs for given user.
        /// </summary>
        /// <returns>List of Thread Bookmarks.</returns>
        /// <param name="forceRefresh">Force Refresh.</param>
        public async Task<List<AwfulPM>> GetAllPrivateMessagesAsync(bool forceRefresh = false)
        {
            var pms = this.context.PrivateMessages.ToList();
            if (!pms.Any() || forceRefresh)
            {
                var threads = await this.manager.GetAllPrivateMessageListAsync().ConfigureAwait(false);
                pms = await this.context.AddAllPrivateMessages(threads).ConfigureAwait(false);
            }

            return pms.OrderBy(n => n.SortOrder).ToList();
        }

        /// <summary>
        /// Send Private Message.
        /// </summary>
        /// <param name="newPrivateMessageEntity">New PM.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendPrivateMessageAsync(NewPrivateMessage newPrivateMessageEntity, CancellationToken token = default)
        {
            return await this.manager.SendPrivateMessageAsync(newPrivateMessageEntity, token).ConfigureAwait(false);
        }
    }
}
