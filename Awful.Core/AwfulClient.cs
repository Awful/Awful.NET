// <copyright file="AwfulClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Net;
using System.Text.Json;
using AngleSharp.Html.Parser;
using Awful.Core.Entities.Bans;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;
using Awful.Core.Utilities;
using Awful.Logger;

namespace Awful.Core
{
    /// <summary>
    /// WebClient for SA requests.
    /// </summary>
    public class AwfulClient : IDisposable
    {
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string Language = "ja,en-US;q=0.8,en;q=0.6";
        private const string Encoding = "gzip, deflate, sdch";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2593.0 Safari/537.36";
        private readonly HttpClientHandler httpClientHandler;
        private bool isDisposed;
        private HtmlParser parser;
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClient"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="authenticationCookie">The users authentication cookie.</param>
        public AwfulClient(ILogger logger, CookieContainer? authenticationCookie = null)
        {
            this.logger = logger;
            this.parser = new HtmlParser();
            if (authenticationCookie != null)
            {
                this.CookieContainer = authenticationCookie;
            }
            else
            {
                this.CookieContainer = new CookieContainer();
            }

            this.httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
                UseDefaultCredentials = false,
                CookieContainer = this.CookieContainer,
            };
            this.Client = new HttpClient(this.httpClientHandler);
            this.Client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = true,
            };

            this.Client.DefaultRequestHeaders.Add("Accept", Accept);
            this.Client.DefaultRequestHeaders.Add("Accept-Language", Language);
            this.Client.DefaultRequestHeaders.Add("Accept-Encoding", Encoding);
            this.Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            this.Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        }

        /// <summary>
        /// Gets or sets if the user is on probation.
        /// </summary>
        public ProbationItem? Probation { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated, based on if they have cookies in the container.
        /// </summary>
        public bool IsAuthenticated => this.CookieContainer != null && this.CookieContainer.Count > 2;

        /// <summary>
        /// Gets the (Actual) HttpClient used to make requests.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets or sets the CookieContainer for the WebClient.
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// GETs data from SA.
        /// </summary>
        /// <param name="endpoint">The endpoint to GET data from.</param>
        /// <param name="shouldBeJson">Checks if the resulting object is JSON.</param>
        /// <param name="token">A CancelationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> GetDataAsync(string endpoint, bool shouldBeJson = false, CancellationToken token = default)
        {
            HttpResponseMessage? result = null;
            string html = string.Empty;
            try
            {
                this.Client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
                result = await this.Client.GetAsync(new Uri(endpoint), token);
                if (result is null)
                {
                    throw new NullReferenceException(nameof(result));
                }

                var resultUri = result.RequestMessage?.RequestUri != null ? result.RequestMessage.RequestUri.AbsoluteUri.ToString() : string.Empty;

                html = await result.ReadHtmlAsync();
                return await this.CheckForErrorsAsync(result, html, resultUri, shouldBeJson);
            }
            catch (AwfulClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AwfulClientException(ex, new Result(result?.IsSuccessStatusCode ?? false, html, endpoint));
            }
        }

        /// <summary>
        /// POST data to SA.
        /// </summary>
        /// <param name="endpoint">The SA Endpoint to POST to.</param>
        /// <param name="data">The FormUrlEncodedContent data.</param>
        /// <param name="shouldBeJson">Checks if the resulting object is JSON.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> PostDataAsync(string endpoint, FormUrlEncodedContent data, bool shouldBeJson = false, CancellationToken token = default)
        {
            HttpResponseMessage? result = null;
            string html = string.Empty;
            try
            {
                this.Client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
                result = await this.Client.PostAsync(new Uri(endpoint), data, token);
                html = await result.ReadHtmlAsync();
                var returnUrl = result.Headers.Location != null ? result.Headers.Location.OriginalString : string.Empty;
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = result.RequestMessage?.RequestUri != null ? result.RequestMessage.RequestUri.ToString() : string.Empty;
                }

                return await this.CheckForErrorsAsync(result, html, returnUrl, shouldBeJson);
            }
            catch (AwfulClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AwfulClientException(ex, new Result(result?.IsSuccessStatusCode ?? false, html, endpoint));
            }
        }

        /// <summary>
        /// Posts SA Form Data.
        /// </summary>
        /// <param name="endpoint">The endpoint URL to post to.</param>
        /// <param name="form">The Form.</param>
        /// <param name="shouldBeJson">Checks if the resulting object is JSON.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> PostFormDataAsync(string endpoint, MultipartFormDataContent form, bool shouldBeJson = false, CancellationToken token = default)
        {
            HttpResponseMessage? result = null;
            string html = string.Empty;
            try
            {
                result = await this.Client.PostAsync(new Uri(endpoint), form, token);
                html = await result.ReadHtmlAsync();
                var resultUri = result.RequestMessage?.RequestUri != null ? result.RequestMessage.RequestUri.AbsoluteUri.ToString() : string.Empty;
                return await this.CheckForErrorsAsync(result, html, endpoint: resultUri, shouldBeJson);
            }
            catch (AwfulClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AwfulClientException(ex, new Result(result?.IsSuccessStatusCode ?? false, html, endpoint));
            }
        }

        /// <summary>
        /// Clears cookies from the current WebClient session.
        /// </summary>
        public void ClearCookies()
        {
            foreach (Cookie cookie in this.httpClientHandler.CookieContainer.GetCookies(new Uri(EndPoints.BaseUrl)))
            {
                cookie.Expired = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose WebClient.
        /// </summary>
        /// <param name="disposing">Has dispose already been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                this.httpClientHandler.Dispose();
            }

            this.isDisposed = true;
        }

        private static void CheckForPaywall(string html)
        {
            if (html.Contains("Sorry, you must be a registered forums member to view this page."))
            {
                throw new PaywallException(Awful.Core.Resources.ExceptionMessages.PaywallThreadHit);
            }
        }

        private async Task<Result> CheckForErrorsAsync(HttpResponseMessage message, string text, string endpoint, bool shouldBeJson = false)
        {
            if (shouldBeJson)
            {
                try
                {
                    return new JsonResult(JsonSerializer.Serialize(text), message.IsSuccessStatusCode, text, endpoint);
                }
                catch (Exception ex)
                {
                    this.logger.Log(ex);
                }
            }

            return await this.CheckHtmlForErrorsAsync(message, text, endpoint);
        }

        private async Task<Result> CheckHtmlForErrorsAsync(HttpResponseMessage message, string text, string endpoint)
        {
            // SA can return HTML errors, even from JSON endpoints.
            // If we get JSON, this will still parse and return a document.
            // So it should be safe to still check it for HTML errors and return it.
            var document = await this.parser.ParseDocumentAsync(text);

            var probationNode = document.QuerySelector("#probation_warn");
            var onProbationText = string.Empty;
            if (probationNode != null)
            {
                onProbationText = probationNode.TextContent.Trim();
            }

            if (document.Body != null && !document.Body.ClassList.Contains("standarderror"))
            {
                return new HtmlResult(document, message.IsSuccessStatusCode, text, endpoint, onProbationText: onProbationText);
            }

            var errorNode = document.QuerySelector(".inner");
            var errorText = string.Empty;
            if (errorNode != null)
            {
                CheckForPaywall(errorNode.TextContent);
                errorText = errorNode.TextContent.Trim();
            }
            else
            {
                errorText = "An error occured in the HTML, but couldn't be parsed.";
            }

            throw new AwfulClientException(new HtmlResult(document, message.IsSuccessStatusCode, text, endpoint, errorText, onProbationText));
        }
    }
}
