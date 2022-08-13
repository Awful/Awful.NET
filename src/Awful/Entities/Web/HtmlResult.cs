// <copyright file="HtmlResult.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;

namespace Awful.Entities.Web
{
    /// <summary>
    /// Html Result.
    /// </summary>
    public class HtmlResult : Result
    {
        public HtmlResult(IHtmlDocument document, bool httpRequestIsSuccess, string text, string endpoint, string errorHtml = "", string onProbationText = "")
            : base(httpRequestIsSuccess, text, endpoint, errorHtml, onProbationText)
        {
            this.Document = document;
        }

        /// <summary>
        /// Gets the HTML Document of the result text,
        /// if the result is HTML.
        /// </summary>
        public IHtmlDocument Document { get; }
    }
}
