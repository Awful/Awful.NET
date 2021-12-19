// <copyright file="BanManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Bans;
using Awful.Core.Exceptions;
using Awful.Core.Utilities;
using Awful.Exceptions;
using Awful.Parser.Handlers;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Bans on Something Awful.
    /// </summary>
    public class BanManager
    {
        private readonly AwfulClient webManager;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public BanManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
        }

        /// <summary>
        /// Get the Banned Members page.
        /// </summary>
        /// <param name="page">The page number to start parsing from. Defaults to 1.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A BanPage.</returns>
        public async Task<BanPage> GetBanPageAsync(int page = 1, CancellationToken token = default)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(string.Format(CultureInfo.InvariantCulture, EndPoints.RapSheet, page), false, token).ConfigureAwait(false);
            try
            {
                if (result?.Document == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find document while parsing Ban Page.", new Awful.Core.Entities.SAItem(result));
                }

                var banPage = BanHandler.ParseBanPage(result.Document);
                banPage.Result = result;
                return banPage;
            }
            catch (Exception ex)
            {
                throw new Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Pings the main forums page and checks if the user under probation.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A ProbationItem.</returns>
        public async Task<ProbationItem> CheckForProbationAsync(CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var result = await this.webManager.GetDataAsync(EndPoints.BaseUrl, false, token).ConfigureAwait(false);
            try
            {
                if (result?.Document == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find document while parsing Probation Page.", new Awful.Core.Entities.SAItem(result));
                }

                var prob = BanHandler.ParseForProbation(result.Document);
                this.webManager.Probation = prob;
                prob.Result = result;
                return prob;
            }
            catch (Exception ex)
            {
                throw new AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
