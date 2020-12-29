// <copyright file="ForumHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Threads;
using Awful.Core.Utilities;

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
        /// <param name="threadList">The Forum Thread List.</param>
        /// <returns>A Forum Thread List with Updated Info.</returns>
        public static ThreadList GetForumPageInfo(IHtmlDocument document, ThreadList threadList)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (threadList == null)
            {
                throw new ArgumentNullException(nameof(threadList));
            }

            var pages = document.QuerySelector(".pages");
            if (pages == null)
            {
                return threadList;
            }

            var select = pages.QuerySelector("select");
            if (select != null)
            {
                var selectedPageItem = select.QuerySelector("option:checked");
                threadList.CurrentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
                threadList.TotalPages = select.ChildElementCount;
            }
            else
            {
                threadList.CurrentPage = 1;
                threadList.TotalPages = 1;
            }

            var testDiv = document.QuerySelector(".mainbodytextlarge");
            if (testDiv != null)
            {
                var parentForumLinks = testDiv.QuerySelectorAll("a");
                if (parentForumLinks.Length >= 1)
                {
                    var parentForumLink = parentForumLinks[1];
                    threadList.ParentForumName = parentForumLink.TextContent;
                    var link = parentForumLink.GetAttribute("href");
                    var queryString = HtmlHelpers.ParseQueryString(link);
                    if (queryString.ContainsKey("forumid"))
                    {
                        threadList.ParentForumId = Convert.ToInt32(queryString["forumid"], CultureInfo.InvariantCulture);
                    }
                }
            }

            var forumLink = document.QuerySelector(".bclast");
            if (forumLink != null)
            {
                threadList.ForumName = forumLink.TextContent;
                var link = forumLink.GetAttribute("href");
                var queryString = HtmlHelpers.ParseQueryString(link);
                if (queryString.ContainsKey("forumid"))
                {
                    threadList.ForumId = Convert.ToInt32(queryString["forumid"], CultureInfo.InvariantCulture);
                }
            }

            return threadList;
        }
    }
}
