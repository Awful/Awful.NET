// <copyright file="SAclopediaManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.SAclopedia;
using Awful.Entities.Web;
using Awful.Exceptions;
using Awful.Handlers;
using Awful.Utilities;

namespace Awful.Managers
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
        public async Task<SAclopediaCategoryList> GetCategoryListAsync(CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase, false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Entities.SAItem(result));
            }

            try
            {
                var categories = SAclopediaHandler.ParseCategoryList(htmlResult.Document);
                var categoryList = new SAclopediaCategoryList(categories);
                categoryList.Result = result;
                return categoryList;
            }
            catch (Exception ex)
            {
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Gets an list of SAclopedia Entry Items.
        /// </summary>
        /// <param name="id">The id of the entry item.</param>
        /// <param name="act">The "act", defaults to 5 (Entry Item).</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of SAclopedia Entry Items.</returns>
        public async Task<SAclopediaEntryItemList> GetEntryItemListAsync(int id, int act = 5, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase + $"?act={act}&l={id}", false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Entities.SAItem(result));
            }

            try
            {
                var entries = SAclopediaHandler.ParseEntryItemList(htmlResult.Document);
                var entryList = new SAclopediaEntryItemList(entries) { Result = result };
                return entryList;
            }
            catch (Exception ex)
            {
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
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
                throw new UserAuthenticationException(Awful.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.SAclopediaBase + $"?act={act}&topicid={id}", false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Entities.SAItem(result));
            }

            try
            {
                var entry = SAclopediaHandler.ParseEntry(htmlResult.Document, id);
                entry.Result = result;
                return entry;
            }
            catch (Exception ex)
            {
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
            }
        }
    }
}
