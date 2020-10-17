// <copyright file="AuthenticationManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Newtonsoft.Json;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Authentication on Something Awful.
    /// </summary>
    public class AuthenticationManager
    {
        private AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationManager"/> class.
        /// </summary>
        /// <param name="web">The SA WebClient.</param>
        public AuthenticationManager(AwfulClient web)
        {
            this.webManager = web;
        }

        /// <summary>
        /// Authenticate a Something Awful user. This does not use the normal "this.webManager" for handling the request
        /// because it requires we return the cookie container, so it can be used for actual authenticated requests.
        /// </summary>
        /// <param name="username">The Something Awful username.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>An auth result object.</returns>
        public async Task<AuthResult> AuthenticateAsync(string username, string password, CancellationToken token = default)
        {
            var dic = new Dictionary<string, string>
            {
                ["action"] = "login",
                ["username"] = username,
                ["password"] = password,
            };
            using var header = new FormUrlEncodedContent(dic);
            var webResult = await this.webManager.PostDataAsync(EndPoints.LoginUrl, header, token).ConfigureAwait(false);
            var authResult = new AuthResult(this.webManager.CookieContainer, true);
            if (string.IsNullOrEmpty(webResult.AbsoluteEndpoint))
            {
                return authResult;
            }

            var location = webResult.AbsoluteEndpoint.StartsWith("\\", StringComparison.InvariantCulture) ? "http:" + webResult.AbsoluteEndpoint : webResult.AbsoluteEndpoint;
            var uri = new Uri(location);

            // TODO: Make DAMN sure that the cookie result and web query string are enough checks to verify being logged in.
            var queryString = HtmlHelpers.ParseQueryString(uri.Query);
            if (!queryString.ContainsKey("loginerror"))
            {
                return await this.FetchUserAsync(authResult, token).ConfigureAwait(false);
            }

            if (queryString["loginerror"] == null)
            {
                return await this.FetchUserAsync(authResult, token).ConfigureAwait(false);
            }

            switch (queryString["loginerror"])
            {
                case "1":
                    authResult.Error = "Failed to enter phrase from the security image.";
                    break;
                case "2":
                    authResult.Error = "The password you entered is wrong. Remember passwords are case-sensitive! Be careful... too many wrong passwords and you will be locked out temporarily.";
                    break;
                case "3":
                    authResult.Error = "The username you entered is wrong, maybe you should try 'idiot' instead? Watch out... too many failed login attempts and you will be locked out temporarily.";
                    break;
                case "4":
                    authResult.Error =
                        "You've made too many failed login attempts. Your IP address is temporarily blocked.";
                    break;
                default:
                    authResult.Error =
                        "Something happened and we couldn't log you in! That's a bummer :(.";
                    break;
            }

            authResult.IsSuccess = false;
            return authResult;
        }

        private async Task<AuthResult> FetchUserAsync(AuthResult authResult, CancellationToken token)
        {
            string url = string.Format(CultureInfo.InvariantCulture, EndPoints.UserProfile, 0);
            var result = await this.webManager.GetDataAsync(url, token).ConfigureAwait(false);
            var user = JsonConvert.DeserializeObject<User>(result.ResultHtml);
            authResult.CurrentUser = user;
            return authResult;
        }
    }
}
