﻿// <copyright file="SmileManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Smilies;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling the Smiles used on Something Awful.
    /// </summary>
    public class SmileManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmileManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public SmileManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Gets all of the smiles used on Something Awful.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of Smile Categories, which includes the smiles.</returns>
        public async Task<SmileCategoryList> GetSmileListAsync(CancellationToken token = default)
        {
            var result = await this.webManager.GetDataAsync(EndPoints.SmileUrl, false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var smiles = SmileHandler.ParseSmileList(htmlResult.Document);
                var smilesList = new SmileCategoryList(smiles) { Result = result };
                return smilesList;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
