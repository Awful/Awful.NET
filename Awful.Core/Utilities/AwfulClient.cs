// <copyright file="AwfulClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Awful.Core.Entities.Bans;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;

namespace Awful.Core.Utilities
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClient"/> class.
        /// </summary>
        /// <param name="authenticationCookie">The users authentication cookie.</param>
        public AwfulClient(CookieContainer authenticationCookie = null)
        {
            this.Parser = new HtmlParser();
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
        public ProbationItem Probation { get; set; } = new ProbationItem();

        /// <summary>
        /// Gets a value indicating whether the user is authenticated, based on if they have cookies in the container.
        /// </summary>
        public bool IsAuthenticated => this.CookieContainer != null && this.CookieContainer.Count > 0;

        /// <summary>
        /// Gets the HTML Parser.
        /// </summary>
        public HtmlParser Parser { get; }

        /// <summary>
        /// Gets the (Actual) HttpClient used to make requests.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets the CookieContainer for the WebClient.
        /// </summary>
        public CookieContainer CookieContainer { get; }

        /// <summary>
        /// GETs data from SA.
        /// </summary>
        /// <param name="endpoint">The endpoint to GET data from.</param>
        /// <param name="token">A CancelationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> GetDataAsync(string endpoint, CancellationToken token = default)
        {
            this.Client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
            var result = await this.Client.GetAsync(new Uri(endpoint), token).ConfigureAwait(false);
            var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            string html = reader.ReadToEnd();
            CheckForPaywall(html);
            return new Result(html, endpoint: result.RequestMessage.RequestUri.AbsoluteUri);
        }

        /// <summary>
        /// POST data to SA.
        /// </summary>
        /// <param name="endpoint">The SA Endpoint to POST to.</param>
        /// <param name="data">The FormUrlEncodedContent data.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> PostDataAsync(string endpoint, FormUrlEncodedContent data, CancellationToken token = default)
        {
            this.Client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
            var result = await this.Client.PostAsync(new Uri(endpoint), data, token).ConfigureAwait(false);
            var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            string html = reader.ReadToEnd();
            CheckForPaywall(html);
            var returnUrl = result.Headers.Location != null ? result.Headers.Location.OriginalString : string.Empty;
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = result.RequestMessage.RequestUri != null ? result.RequestMessage.RequestUri.ToString() : string.Empty;
            }

            return new Result(html, endpoint: returnUrl);
        }

        /// <summary>
        /// Posts SA Form Data.
        /// </summary>
        /// <param name="endpoint">The endpoint URL to post to.</param>
        /// <param name="form">The Form.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> PostFormDataAsync(string endpoint, MultipartFormDataContent form, CancellationToken token = default)
        {
            var result = await this.Client.PostAsync(new Uri(endpoint), form, token).ConfigureAwait(false);
            var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            var html = reader.ReadToEnd();
            CheckForPaywall(html);
            var newResult = new Result(html, endpoint: result.RequestMessage.RequestUri.ToString());
            return newResult;
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
    }
}
