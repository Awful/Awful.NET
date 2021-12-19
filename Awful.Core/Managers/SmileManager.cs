// <copyright file="SmileManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Smilies;
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
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmileManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public SmileManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all of the smiles used on Something Awful.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of Smile Categories, which includes the smiles.</returns>
        public async Task<SmileCategoryList> GetSmileListAsync(CancellationToken token = default)
        {
            var result = await this.webManager.GetDataAsync(EndPoints.SmileUrl, false, token).ConfigureAwait(false);
            try
            {
                var smiles = SmileHandler.ParseSmileList(result.Document);
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
