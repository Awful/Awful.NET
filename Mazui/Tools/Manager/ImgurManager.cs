using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.Storage.Streams;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Microsoft.QueryStringDotNET;

namespace Mazui.Tools.Manager
{
    public class ImgurManager
    {
        private readonly ImgurClient _client = new ImgurClient("e5c018ac1f4c157", "74590241e221d89c8dbf15fa74fc3ead27e9aaaa");

        public async Task<string> ImgurLogin()
        {
            try
            {
                var endpoint = new OAuth2Endpoint(_client);
                var redirectUrl = endpoint.GetAuthorizationUrl(OAuth2ResponseType.Token);
                var callbackUri =
                  WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();

                var authenticationResult =
                  await
                    WebAuthenticationBroker.AuthenticateAsync(
                    WebAuthenticationOptions.None,
                    new Uri(redirectUrl.ToLower()), new Uri(callbackUri));
                return ParseAuthenticationResult(authenticationResult);
            }
            catch (Exception)
            {

            }
            return string.Empty;
        }

        public async Task<IOAuth2Token> UpdateImgurTokens(string username, string refreshToken)
        {
            var endpoint = new OAuth2Endpoint(_client);
            return await endpoint.GetTokenByRefreshTokenAsync(refreshToken);
        }

        public async Task<IImage> UploadImage(IRandomAccessStream stream, string username)
        {
            var accessToken = await GetTokens(username);
            var endpoint = new ImageEndpoint(new ImgurClient("e5c018ac1f4c157", "74590241e221d89c8dbf15fa74fc3ead27e9aaaa", new OAuth2Token(accessToken, "", "", "", "", Int32.MinValue)));
            return await endpoint.UploadImageStreamAsync(stream.AsStream());
        }

        public string ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine("Error");
                    break;
                case WebAuthenticationStatus.Success:
                    var querystring = QueryString.Parse(result.ResponseData.Split('#')[1]);

                    var access_token = querystring["access_token"];
                    var refresh_token = querystring["refresh_token"];
                    var expires_in = querystring["expires_in"];
                    var account_username = querystring["account_username"];

                    if (string.IsNullOrEmpty(account_username)) return string.Empty;
                    var tokenExpiry = DateTime.Now.AddSeconds(double.Parse(expires_in));
                    SaveTokens(account_username, tokenExpiry, access_token, refresh_token);
                    return account_username;
                case WebAuthenticationStatus.UserCancel:
                    Debug.WriteLine("Operation aborted");
                    break;
                default:
                    break;
            }
            return string.Empty;
        }

        public void SaveTokens(string username, DateTime expiration, string accessToken, string refreshToken)
        {
            try
            {
                var vault = new PasswordVault();

                var credential = new PasswordCredential(
                    "imgur",
                    username,
                    string.Format("{0}|{1}|{2}", expiration, accessToken, refreshToken));
                vault.Add(credential);
            }
            catch (Exception)
            {

            }
        }

        public async Task<string> GetTokens(string username)
        {
            try
            {
                var vault = new PasswordVault();
                var result = vault.Retrieve("imgur", username);
                var splitToken = result.Password.Split('|');
                var tokenExpiry = DateTime.Parse(splitToken[0]);
                var accessToken = splitToken[1];
                var refreshToken = splitToken[2];
                if (DateTime.UtcNow <= tokenExpiry) return accessToken;
                vault.Remove(result);
                var newAccessToken = await UpdateImgurTokens(username, refreshToken);
                SaveTokens(username, DateTime.UtcNow.AddMonths(1), newAccessToken.AccessToken, newAccessToken.RefreshToken);
                return newAccessToken.AccessToken;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void RemoveTokens(string username)
        {
            var vault = new PasswordVault();
            try
            {
                // Removes the credential from the password vault.
                vault.Remove(vault.Retrieve("imgur", username));
            }
            catch (Exception)
            {
                // If no credentials have been stored with the given RESOURCE_NAME, an exception
                // is thrown.
            }
        }
    }
}
