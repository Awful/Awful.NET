// <copyright file="SigninAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;
using Awful;
using Awful.Entities.Web;
using Awful.Managers;
using Awful.UI.Events;
using Awful.UI.Entities;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Signin Action.
    /// </summary>
    public class SigninAction
    {
        private IDatabaseContext context;

        private IPlatformServices platformServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigninAction"/> class.
        /// Handles signing in, out.
        /// </summary>
        /// <param name="platformServices">Platform Properties.</param>
        /// <param name="context">Awful Database Context.</param>
        public SigninAction(IPlatformServices platformServices, IDatabaseContext context)
        {
            if (platformServices == null)
            {
                throw new ArgumentNullException(nameof(platformServices));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.platformServices = platformServices;
            this.context = context;
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
        public event SigninEventHandler? OnSignin;

        /// <summary>
        /// Gets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn { get { return CurrentUser != null; } }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public UserAuth? CurrentUser { get; private set; }

        /// <summary>
        /// Signs in the current user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>A AuthResult object.</returns>
        public async Task<AuthResult> SigninAsync(string username, string password, CancellationToken token = default)
        {
            using var client = new AwfulClient();
            var manager = new AuthenticationManager(client);
            var result = await manager.AuthenticateAsync(username, password, token).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                if (result.CurrentUser?.Usertitle is not null)
                {
                    var captures = Regex.Match(result.CurrentUser.Usertitle, @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?");
                    var avatarLink = captures.Captures.Count > 0 ? captures.Captures[0].Value : string.Empty;
                    CurrentUser = new UserAuth { RecievePM = result.CurrentUser.Receivepm, AvatarLink = avatarLink, IsDefaultUser = true, UserName = result.CurrentUser?.Username ?? string.Empty, CookiePath = platformServices.CookiePath };
                    await context.AddOrUpdateUserAsync(CurrentUser).ConfigureAwait(false);
                    platformServices.CookieManager.SaveCookie(result.AuthenticationCookieContainer, CurrentUser.CookiePath);
                }
            }

            OnSignin?.Invoke(this, new SigninEventArgs(IsSignedIn, CurrentUser));
            return result;
        }
    }
}
