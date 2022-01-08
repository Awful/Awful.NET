// <copyright file="BanManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Bans;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Awful.Exceptions;
using System.Globalization;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Bans on Something Awful.
    /// </summary>
    public class BanManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public BanManager(AwfulClient webManager)
        {
            this.webManager = webManager;
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

            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document while parsing Ban Page.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var banPage = BanHandler.ParseBanPage(htmlResult.Document);
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
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document while parsing Probation Page.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var prob = BanHandler.ParseForProbation(htmlResult.Document);
                this.webManager.SetProbation(prob);
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
