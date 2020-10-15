// <copyright file="Result.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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
        /// <param name="html">The HTML of the request.</param>
        /// <param name="json">The JSON of the request.</param>
        /// <param name="endpoint">The endpoint that was hit.</param>
        public Result(string html = "", string json = "", string endpoint = "")
        {
            this.ResultHtml = html;
            this.ResultJson = json;
            this.AbsoluteEndpoint = endpoint;
        }

        /// <summary>
        /// Gets the result of the request, in HTML form.
        /// </summary>
        public string ResultHtml { get; }

        /// <summary>
        /// Gets the Uri of the request.
        /// </summary>
        public string AbsoluteEndpoint { get; }

        /// <summary>
        /// Gets the result of the request, in JSON form.
        /// </summary>
        public string ResultJson { get; }
    }
}
