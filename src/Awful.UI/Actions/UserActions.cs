// <copyright file="UserActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.JSON;
using Awful.Managers;
using Awful.Themes;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// User Actions.
    /// </summary>
    public class UserActions
    {
        private IDatabaseContext context;
        private UserManager manager;
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="templates">Templates.</param>
        public UserActions(AwfulClient client, IDatabaseContext context, ITemplateHandler templates)
        {
            manager = new UserManager(client);
            this.context = context;
            this.templates = templates;
        }

        /// <summary>
        /// Gets logged in user from their profile page.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A User.</returns>
        public async Task<User?> GetLoggedInUserAsync(CancellationToken token = default)
        {
            return await manager.GetUserFromProfilePageAsync(0, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a user from their profile page.
        /// </summary>
        /// <param name="userId">The User Id. 0 gets the current logged in user.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A User.</returns>
        public async Task<User?> GetUserFromProfilePageAsync(long userId, CancellationToken token = default)
        {
            return await manager.GetUserFromProfilePageAsync(userId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders User Profile as HTML string.
        /// </summary>
        /// <param name="user">SA User.</param>
        /// <param name="defaultOptions">Default Webview Options.</param>
        /// <returns>HTML string.</returns>
        public string RenderProfileView(User user, DefaultOptions defaultOptions)
        {
            return templates.RenderProfileView(user, defaultOptions);
        }
    }
}
