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
        public async Task<AwfulForum> SetIsFavoriteForumAsync(AwfulForum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            var realForum = this.context.Forums.FirstOrDefault(n => n.Id == forum.Id);
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
        /// Save existing forum.
        /// </summary>
        /// <param name="forum">Forum to update.</param>
        /// <returns>Forum.</returns>
        public async Task<AwfulForum> SetShowSubforumsForumAsync(AwfulForum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            var realForum = this.context.Forums.FirstOrDefault(n => n.Id == forum.Id);
            if (realForum == null)
            {
                return forum;
            }

            forum.IsShowSubForumsVisible = !forum.IsShowSubForumsVisible;
            realForum.IsFavorited = !realForum.IsFavorited;
            await this.context.UpdateForumAsync(realForum).ConfigureAwait(false);
            return forum;
        }

        /// <summary>
        /// Setup Favorites Groups.
        /// </summary>
        /// <param name="groups">Forum Groups.</param>
        /// <param name="forum">Forum to be favorites.</param>
        /// <returns>List of Groups.</returns>
        public async Task<ObservableCollection<ForumGroup>> SetupFavoritesAsync(ObservableCollection<ForumGroup> groups, AwfulForum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            forum = await this.SetIsFavoriteForumAsync(forum).ConfigureAwait(false);
            var favorites = groups.FirstOrDefault(n => n.Id == 0);
            if (favorites != null && forum.IsFavorited)
            {
                favorites.Add(forum);
            }
            else if (favorites != null && !forum.IsFavorited)
            {
                favorites.Remove(forum);
            }

            return groups;
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

            var favorited = awfulCatList.SelectMany(n => n.SubForums).Where(y => y.IsFavorited);
            var favoriteCat = new Forum() { SubForums = favorited.ToList(), Id = 0, SortOrder = 0, Title = "Favorites" };
            awfulCatList.Insert(0, favoriteCat);

            return awfulCatList;
        }
    }
}
