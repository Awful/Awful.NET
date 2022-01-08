// <copyright file="ForumHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;
using Awful.Core.Entities.Forums;
using Awful.Core.Exceptions;
using Awful.Core.Utilities;
using System.Globalization;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// SA Forum Handler.
    /// </summary>
    public static class ForumHandler
    {
        /// <summary>
        /// Get Forum Page Info.
        /// </summary>
        /// <param name="document">The SA Forum Page.</param>
        /// <returns>A Forum Thread List with Updated Info.</returns>
        public static Forum GetForumInfoViaThreadListPage(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var currentPage = 0;
            var totalPages = 0;

            var pages = document.QuerySelector(".pages");
            if (pages == null)
            {
                throw new AwfulParserException($"GetForumPageInfo: pages");
            }

            var select = pages.QuerySelector("select");

            if (select is null)
            {
                throw new AwfulParserException($"GetForumPageInfo: select");
            }

            var selectedPageItem = select.QuerySelector("option:checked");
            if (selectedPageItem == null)
            {
                throw new AwfulParserException($"GetForumPageInfo: selectedPageItem");
            }

            currentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
            totalPages = select.ChildElementCount;

            string parentForumName = string.Empty;
            int parentForumId = 0;

            var testDiv = document.QuerySelector(".mainbodytextlarge");
            var parentForumLinks = testDiv?.QuerySelectorAll("a");
            if (parentForumLinks is not null && parentForumLinks.Length >= 1)
            {
                var parentForumLink = parentForumLinks[1];
                parentForumName = parentForumLink.TextContent;
                var link = parentForumLink.TryGetAttribute("href");
                var queryString = link.ParseQueryString();
                if (queryString.ContainsKey("forumid"))
                {
                    parentForumId = Convert.ToInt32(queryString["forumid"], CultureInfo.InvariantCulture);
                }
            }

            var forumLink = document.QuerySelector(".bclast");
            if (forumLink is null)
            {
                throw new AwfulParserException($"GetForumPageInfo: .bclast");
            }

            var forumName = forumLink.TextContent;
            var fLink = forumLink.TryGetAttribute("href");
            var fqueryString = fLink.ParseQueryString();
            fqueryString.TryGetValue("forumid", out var forumIdQuery);
            if (forumIdQuery is null)
            {
                throw new AwfulParserException($"GetForumPageInfo: forumIdQuery");
            }

            var forumId = Convert.ToInt32(forumIdQuery, CultureInfo.InvariantCulture);
            return new Forum(forumId, forumName, parentForumId, parentForumName);
        }
    }
}
