// <copyright file="SigninAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Managers;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Events;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Signin Action.
    /// </summary>
    public class SigninAction
    {
        private AwfulContext context;
        private IPlatformProperties platformProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigninAction"/> class.
        /// Handles signing in, out.
        /// </summary>
        /// <param name="platformProperties">Platform Properties.</param>
        /// <param name="context">Awful Database Context.</param>
        public SigninAction(IPlatformProperties platformProperties, AwfulContext context)
        {
            if (platformProperties == null)
            {
                throw new ArgumentNullException(nameof(platformProperties));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.platformProperties = platformProperties;
            this.context = context;
            this.CurrentUser = this.context.Users.FirstOrDefault();
        }

        /// <summary>
        /// Handles Signin Events.
        /// </summary>
        /// <param name="sender">Sender of request.</param>
        /// <param name="e"><see cref="SigninEventArgs"/>.</param>
        public delegate void SigninEventHandler(object sender, SigninEventArgs e);

        /// <summary>
        /// Handle Events.
        /// </summary>
        public event SigninEventHandler OnSignin;

        /// <summary>
        /// Gets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn { get { return this.CurrentUser != null; } }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public UserAuth CurrentUser { get; private set; }

        /// <summary>
        /// Signs in the current user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>A AuthResult object.</returns>
        public async Task<AuthResult> SigninAsync(string username, string password)
        {
            using var client = new AwfulClient();
            var manager = new AuthenticationManager(client);
            var result = await manager.AuthenticateAsync(username, password).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                var captures = Regex.Match(result.CurrentUser.Usertitle, @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?");
                var avatarLink = captures.Captures.Count > 0 ? captures.Captures[0].Value : string.Empty;
                this.CurrentUser = new UserAuth { AvatarLink = avatarLink, IsDefaultUser = true, UserName = result.CurrentUser.Username, CookiePath = this.platformProperties.CookiePath };
                await this.context.AddOrUpdateUserAsync(this.CurrentUser).ConfigureAwait(false);
                CookieManager.SaveCookie(result.AuthenticationCookieContainer, this.CurrentUser.CookiePath);
            }

            this.OnSignin?.Invoke(this, new SigninEventArgs() { IsSignedIn = this.IsSignedIn, User = this.CurrentUser });
            return result;
        }
    }
}
