// <copyright file="SAclopediaManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Awful.Exceptions;

namespace Awful.Core.Managers
{
    /// <summary>
    /// SAclopedia Manager.
    /// </summary>
    public class SAclopediaManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public SAclopediaManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Get SAclopedia Categories.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of SAclopedia Categories.</returns>
        public async Task<List<SAclopediaCategory>> GetCategoryListAsync(CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase, false, token).ConfigureAwait(false);
            try
            {
                return SAclopediaHandler.ParseCategoryList(result.Document);
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Gets an list of SAclopedia Entry Items.
        /// </summary>
        /// <param name="id">The id of the entry item.</param>
        /// <param name="act">The "act", defaults to 5 (Entry Item).</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of SAclopedia Entry Items.</returns>
        public async Task<List<SAclopediaEntryItem>> GetEntryItemListAsync(int id, int act = 5, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase + $"?act={act}&l={id}", false, token).ConfigureAwait(false);
            try
            {
                return SAclopediaHandler.ParseEntryItemList(result.Document);
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Gets an SAclopedia Entry.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <param name="act">The "act", defaults to 3 (Entry).</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>An SAclopedia Entry.</returns>
        public async Task<SAclopediaEntry> GetEntryAsync(int id, int act = 3, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase + $"?act={act}&topicid={id}", false, token).ConfigureAwait(false);
            try
            {
                var entry = SAclopediaHandler.ParseEntry(result.Document, id);
                entry.Result = result;
                return entry;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
