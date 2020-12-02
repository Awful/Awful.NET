// <copyright file="Result.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Awful.Core.Entities.Web
{
    /// <summary>
    /// The SA WebClient Result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="message">The request.</param>
        /// <param name="text">The text of the request.</param>
        /// <param name="endpoint">The endpoint that was hit.</param>
        public Result(HttpResponseMessage message, string text = "", string endpoint = "")
        {
            this.Message = message;
            this.ResultText = text;
            this.AbsoluteEndpoint = endpoint;
        }

        /// <summary>
        /// Gets the raw HTTP Response Message.
        /// </summary>
        public HttpResponseMessage Message { get; }

        /// <summary>
        /// Gets the result of the request.
        /// </summary>
        public string ResultText { get; }

        /// <summary>
        /// Gets or sets the HTML Document of the result text,
        /// if the result is HTML.
        /// </summary>
        public IHtmlDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the JSON object,
        /// if the result is JSON.
        /// </summary>
        public object Json { get; set; }

        /// <summary>
        /// Gets or sets the error text.
        /// If it exists in the resulting HTML.
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        /// Gets the Uri of the request.
        /// </summary>
        public string AbsoluteEndpoint { get; }

        /// <summary>
        /// Gets a value indicating whether the request is successful.
        /// Sometimes the request returns as a success (200) but there's
        /// actual error text on the screen.
        /// So if we set the error text to something, the request actually failed.
        /// </summary>
        public bool IsSuccess => this.Message != null && this.Message.IsSuccessStatusCode && string.IsNullOrEmpty(this.ErrorText);
    }
}
