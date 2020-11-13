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

        public async Task<List<AwfulForumCategory>> GetForumListAsync(bool forceReload, CancellationToken token = default)
        {
            var indexPageSorted = await this.manager.GetSortedIndexPageAsync().ConfigureAwait(false);
            var awfulCatList = new List<AwfulForumCategory>();
            for (int i = 0; i < indexPageSorted.ForumCategories.Count; i++)
            {
                Forum category = indexPageSorted.ForumCategories[i];
                var awfulCategory = new AwfulForumCategory()
                {
                    Id = i,
                    Title = category.Title,
                    SortOrder = i,
                    Forums = category.SubForums.Select(n => new AwfulForum(n) { ForumCategoryId = i }).ToList(),
                };
                awfulCatList.Add(awfulCategory);
            }

            return awfulCatList;
        }
    }
}
