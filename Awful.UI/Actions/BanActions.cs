// <copyright file="BanActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Bans;
using Awful.Core.Entities.Threads;
using Awful.Core.Managers;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Ban Actions.
    /// </summary>
    public class BanActions
    {
        private IAwfulContext context;
        private BanManager manager;
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public BanActions(AwfulClient client, IAwfulContext context, ITemplateHandler templates)
        {
            this.manager = new BanManager(client);
            this.context = context;
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
            return await this.manager.GetBanPageAsync(page, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Render Ban View.
        /// </summary>
        /// <param name="page">Ban Page.</param>
        /// <param name="options">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderBanView(BanPage page, DefaultOptions options)
        {
            return this.templates.RenderBanView(page, options);
        }

        /// <summary>
        /// Pings the main forums page and checks if the user under probation.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A ProbationItem.</returns>
        public async Task<ProbationItem> CheckForProbationAsync(CancellationToken token = default)
        {
            return await this.manager.CheckForProbationAsync(token).ConfigureAwait(false);
        }
    }
}
