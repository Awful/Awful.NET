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

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase, token).ConfigureAwait(false);
            var document = await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false);
            return SAclopediaHandler.ParseCategoryList(document);
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

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase + $"?act={act}&l={id}", token).ConfigureAwait(false);
            var document = await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false);
            return SAclopediaHandler.ParseEntryItemList(document);
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

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase + $"?act={act}&topicid={id}", token).ConfigureAwait(false);
            var document = await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false);
            return SAclopediaHandler.ParseEntry(document, id);
        }
    }
}
