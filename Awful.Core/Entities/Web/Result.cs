// <copyright file="Result.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

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
        /// <param name="html">The HTML of the request.</param>
        /// <param name="json">The JSON of the request.</param>
        /// <param name="endpoint">The endpoint that was hit.</param>
        public Result(HttpResponseMessage message, string html = "", string endpoint = "")
        {
            this.Message = message;
            this.ResultHtml = html;
            this.AbsoluteEndpoint = endpoint;
        }

        /// <summary>
        /// Gets the raw HTTP Response Message.
        /// </summary>
        public HttpResponseMessage Message { get; }

        /// <summary>
        /// Gets the result of the request, in HTML form.
        /// </summary>
        public string ResultHtml { get; }

        /// <summary>
        /// Gets the Uri of the request.
        /// </summary>
        public string AbsoluteEndpoint { get; }

        /// <summary>
        /// Gets a value indicating whether the request is successful.
        /// </summary>
        public bool IsSuccess => this.Message != null && this.Message.IsSuccessStatusCode;
    }
}
