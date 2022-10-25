// <copyright file="BanActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.Bans;
using Awful.Managers;
using Awful.Themes;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Ban Actions.
    /// </summary>
    public class BanActions
    {
        private BanManager manager;
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="templates">Templates.</param>
        public BanActions(AwfulClient client, ITemplateHandler templates)
        {
            manager = new BanManager(client);
            this.templates = templates;
        }

        /// <summary>
        /// Get the Banned Members page.
        /// </summary>
        /// <param name="page">The page number to start parsing from. Defaults to 1.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A BanPage.</returns>
        public async Task<BanPage> GetBanPageAsync(int page = 1, CancellationToken token = default)
        {
            return await manager.GetBanPageAsync(page, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Render Ban View.
        /// </summary>
        /// <param name="page">Ban Page.</param>
        /// <param name="options">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderBanView(BanPage page, DefaultOptions options)
        {
            return templates.RenderBanView(page, options);
        }

        /// <summary>
        /// Pings the main forums page and checks if the user under probation.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A ProbationItem.</returns>
        public async Task<ProbationItem> CheckForProbationAsync(CancellationToken token = default)
        {
            return await manager.CheckForProbationAsync(token).ConfigureAwait(false);
        }
    }
}
