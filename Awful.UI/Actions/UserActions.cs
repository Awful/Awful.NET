// <copyright file="UserActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Smilies;
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
    /// User Actions.
    /// </summary>
    public class UserActions
    {
        private IAwfulContext context;
        private UserManager manager;
        private TemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public UserActions(AwfulClient client, IAwfulContext context, TemplateHandler templates)
        {
            this.manager = new UserManager(client);
            this.context = context;
            this.templates = templates;
        }

        /// <summary>
        /// Gets logged in user from their profile page.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A User.</returns>
        public async Task<User> GetLoggedInUserAsync(CancellationToken token = default)
        {
            return await this.manager.GetUserFromProfilePageAsync(0, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a user from their profile page.
        /// </summary>
        /// <param name="userId">The User Id. 0 gets the current logged in user.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A User.</returns>
        public async Task<User> GetUserFromProfilePageAsync(long userId, CancellationToken token = default)
        {
            return await this.manager.GetUserFromProfilePageAsync(userId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders User Profile as HTML string.
        /// </summary>
        /// <param name="user">SA User.</param>
        /// <param name="defaultOptions">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderProfileView(User user, DefaultOptions defaultOptions)
        {
            return this.templates.RenderProfileView(user, defaultOptions);
        }
    }
}
