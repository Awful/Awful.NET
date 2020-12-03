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
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Microsoft.EntityFrameworkCore;

namespace Awful.UI.Actions
{
    /// <summary>
    /// SAclopedia Action.
    /// </summary>
    public class SAclopediaAction
    {
        private AwfulContext context;
        private SAclopediaManager manager;
        private TemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaAction"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public SAclopediaAction(AwfulClient client, AwfulContext context, TemplateHandler templates)
        {
            this.manager = new SAclopediaManager(client);
            this.context = context;
            this.templates = templates;
        }

        /// <summary>
        /// Generates SAclopedia Entry HTML.
        /// </summary>
        /// <param name="entry">SAclopedia Entry.</param>
        /// <param name="defaultOptions">The default options for HTML template.</param>
        /// <returns>HTML string.</returns>
        public string GenerateSAclopediaEntryTemplate(SAclopediaEntry entry, DefaultOptions defaultOptions = default)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return this.templates.RenderSAclopediaView(entry, defaultOptions);
        }

        /// <summary>
        /// Load SAclopedia Entry.
        /// </summary>
        /// <param name="item">SAclopediaEntryItem.</param>
        /// <param name="token">CancellationToken.</param>
        /// <returns>SAclopediaEntry.</returns>
        public async Task<SAclopediaEntry> LoadSAclopediaEntryAsync(SAclopediaEntryItem item, CancellationToken token = default)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return await this.manager.GetEntryAsync(item.Id, token: token).ConfigureAwait(false);
        }

        /// <summary>
        /// Load SAclopedia Entry Items.
        /// </summary>
        /// <param name="forceRefresh">Force cached refresh into database.</param>
        /// <param name="token">CancellationToken.</param>
        /// <returns>List of SAclopediaEntryItems.</returns>
        public async Task<List<SAclopediaEntryItem>> LoadSAclopediaEntryItemsAsync(bool forceRefresh = false, CancellationToken token = default)
        {
            var list = await this.context.SAclopediaEntryItems.ToListAsync().ConfigureAwait(false);
            if (forceRefresh || !list.Any())
            {
                await this.context.RemoveAllSAclopediaEntryAsync().ConfigureAwait(false);
                list = new List<SAclopediaEntryItem>();
                var categoryList = await this.manager.GetCategoryListAsync(token).ConfigureAwait(false);
                foreach (var category in categoryList.SAclopediaCategories)
                {
                    var entryItemList = await this.manager.GetEntryItemListAsync(category.Id, token: token).ConfigureAwait(false);
                    list.AddRange(entryItemList.SAclopediaEntryItems);
                }

                var test = list.Distinct().ToList();
                await this.context.AddAllSAclopediaEntry(list.Distinct().ToList()).ConfigureAwait(false);
            }

            return list.OrderBy(n => n.Title).ToList();
        }
    }
}
