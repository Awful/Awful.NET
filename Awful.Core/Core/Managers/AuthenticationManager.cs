using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Awful.Models.Web;
using Awful.Tools;
using Newtonsoft.Json;

namespace Awful.Managers
{
    public class AuthenticationManager
    {
        public AuthenticationManager(WebManager web)
        {
            webManager = web;
        }

        WebManager webManager;

        /// <summary>
        /// Authenticate a Something Awful user. This does not use the normal "WebManager" for handling the request
        /// because it requires we return the cookie container, so it can be used for actual authenticated requests.
        /// </summary>
        /// <param name="username">The Something Awful username.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="checkResult">Check the query string for login errors. Default is True.</param>
        /// <returns>An auth result object.</returns>
        public async Task<AuthResult> AuthenticateAsync(string username, string password, bool checkResult = true) {

            var dic = new Dictionary<string, string>
            {
                ["action"] = "login",
                ["username"] = username,
                ["password"] = password
            };
            var header = new FormUrlEncodedContent(dic);
            try
            {
                var webResult = await webManager.PostDataAsync(EndPoints.LoginUrl, header);
                var authResult = new AuthResult(webManager.CookieContainer, webResult.IsSuccess);
                if (string.IsNullOrEmpty(webResult.AbsoluteUri)) return authResult;
                var location = "http:" + webResult.AbsoluteUri;
                var uri = new Uri(location);
                // TODO: Make DAMN sure that the cookie result and web query string are enough checks to verify being logged in.
                var queryString = HtmlHelpers.ParseQueryString(uri.Query);
                if (!queryString.ContainsKey("loginerror")) return authResult;
                if (queryString["loginerror"] == null) return authResult;
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
            catch (Exception ex)
            {
                return new AuthResult(webManager.CookieContainer, false, ex.Message);
            }
        }
    }
}
