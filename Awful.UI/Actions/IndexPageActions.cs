// <copyright file="IndexPageActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Awful.UI.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Microsoft.EntityFrameworkCore;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Main Forum Actions.
    /// </summary>
    public class IndexPageActions
    {
        private IAwfulContext context;
        private IndexPageManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPageActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public IndexPageActions(AwfulClient client, IAwfulContext context)
        {
            this.manager = new IndexPageManager(client);
            this.context = context;
        }

        /// <summary>
        /// Save existing forum.
        /// </summary>
        /// <param name="forum">Forum to update.</param>
        /// <returns>Forum.</returns>
        public async Task<AwfulForum> SetIsFavoriteForumAsync(AwfulForum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            var realForum = await this.context.GetForumAsync(forum.Id).ConfigureAwait(false);
            if (realForum == null)
            {
                return forum;
            }

            forum.IsFavorited = !forum.IsFavorited;
            realForum.IsFavorited = !realForum.IsFavorited;
            await this.context.UpdateForumAsync(realForum).ConfigureAwait(false);
            return forum;
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
                var indexPageSorted = await this.manager.GetSortedIndexPageAsync(token).ConfigureAwait(false);
                awfulCatList = indexPageSorted.ForumCategories;
                await this.context.AddOrUpdateForumCategoriesAsync(awfulCatList).ConfigureAwait(false);
            }

            return awfulCatList;
        }
    }
}
