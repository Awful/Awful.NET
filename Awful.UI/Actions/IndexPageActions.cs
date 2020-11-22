// <copyright file="IndexPageActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
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
    /// Main Forum Actions.
    /// </summary>
    public class IndexPageActions
    {
        private AwfulContext context;
        private IndexPageManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPageActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public IndexPageActions(AwfulClient client, AwfulContext context)
        {
            this.manager = new IndexPageManager(client);
            this.context = context;
        }

        /// <summary>
        /// Save existing forum.
        /// </summary>
        /// <param name="forum">Forum to update.</param>
        /// <returns>Forum.</returns>
        public async Task<Forum> SetIsFavoriteForumAsync(Forum forum)
        {
            var realForum = this.context.Forums.FirstOrDefault(n => n.Id == forum.Id);
            if (realForum == null)
            {
                return forum;
            }

            realForum.IsFavorited = !realForum.IsFavorited;
            return await this.context.UpdateForumAsync(realForum).ConfigureAwait(false);
        }

        /// <summary>
        /// Save existing forum.
        /// </summary>
        /// <param name="forum">Forum to update.</param>
        /// <returns>Forum.</returns>
        public async Task<Forum> SetShowSubforumsForumAsync(Forum forum)
        {
            var realForum = this.context.Forums.FirstOrDefault(n => n.Id == forum.Id);
            if (realForum == null)
            {
                return forum;
            }

            realForum.IsShowSubForumsVisible = !realForum.IsShowSubForumsVisible;
            return await this.context.UpdateForumAsync(realForum).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the forums category list.
        /// </summary>
        /// <param name="forceReload">Force Reloading.</param>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>List of Awful Forum Categories.</returns>
        public async Task<List<Forum>> GetForumListAsync(bool forceReload, CancellationToken token = default)
        {
            var awfulCatList = await this.context.GetForumCategoriesAsync().ConfigureAwait(false);
            if (!awfulCatList.Any() || forceReload)
            {
                var indexPageSorted = await this.manager.GetSortedIndexPageAsync().ConfigureAwait(false);
                awfulCatList = indexPageSorted.ForumCategories;
                await this.context.AddOrUpdateForumCategories(awfulCatList).ConfigureAwait(false);
            }

            return awfulCatList;
        }
    }
}
