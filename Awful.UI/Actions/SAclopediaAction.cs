// <copyright file="SAclopediaAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Awful.Database.Context;

namespace Awful.UI.Actions
{
    /// <summary>
    /// SAclopedia Action.
    /// </summary>
    public class SAclopediaAction
    {
        private AwfulContext context;
        private SAclopediaManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaAction"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public SAclopediaAction(AwfulClient client, AwfulContext context)
        {
            this.manager = new SAclopediaManager(client);
            this.context = context;
        }

        public async Task<List<SAclopediaEntryItem>> LoadSAclopediaEntryItemsAsync(bool forceRefresh = false, CancellationToken token = default)
        {
            var list = this.context.SAclopediaEntryItems.ToList();
            if (!list.Any() || forceRefresh)
            {
                await this.context.RemoveAllSAclopediaEntryAsync().ConfigureAwait(false);
                list = new List<SAclopediaEntryItem>();
                var categoryList = await this.manager.GetCategoryListAsync(token).ConfigureAwait(false);
                foreach (var category in categoryList)
                {
                    var entryItemList = await this.manager.GetEntryItemListAsync(category.Id, token: token).ConfigureAwait(false);
                    list.AddRange(entryItemList);
                }

                var test = list.Distinct().ToList();
                await this.context.AddAllSAclopediaEntry(list.Distinct().ToList()).ConfigureAwait(false);
            }

            return list.OrderBy(n => n.Title).ToList();
        }
    }
}
