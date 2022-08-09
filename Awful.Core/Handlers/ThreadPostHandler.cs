// <copyright file="ThreadPostHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Threads;
using Awful.Core.Exceptions;
using Awful.Core.Utilities;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles SAclopedia Elements.
    /// </summary>
    public static class ThreadPostHandler
    {
        /// <summary>
        /// Parses a thread for posts.
        /// </summary>
        /// <param name="doc">An IHtmlDocument with the thread posts.</param>
        /// <param name="responseEndpoint">The endpoint of the response.</param>
        /// <returns>A thread.</returns>
        public static ThreadPost ParseThread(IHtmlDocument doc, string responseEndpoint = "")
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            PaywallHandler.CheckPaywall(doc);

            if (doc.Title is null)
            {
                throw new AwfulParserException($"ParseThread: title");
            }

            var title = doc.Title.Replace(" - The Something Awful Forums", string.Empty);

            var threadBody = doc.QuerySelector("body");
            if (threadBody is null)
            {
                throw new AwfulParserException($"ParseThread: body");
            }

            var threadId = Convert.ToInt32(threadBody.TryGetAttribute("data-thread"), CultureInfo.InvariantCulture);
            var forumId = Convert.ToInt32(threadBody.TryGetAttribute("data-forum"), CultureInfo.InvariantCulture);

            var threadDivTableHolder = doc.QuerySelector("#thread");
            if (threadDivTableHolder is null)
            {
                throw new AwfulParserException($"ParseThread: #thread");
            }

            var posts = new List<Post>();

            foreach (var threadTable in threadDivTableHolder.QuerySelectorAll("table.post"))
            {
                if (string.IsNullOrEmpty(threadTable.Id?.Replace("post", string.Empty)))
                {
                    continue;
                }

                posts.Add(PostHandler.ParsePost(doc, threadTable));
            }

            var archiveButton = doc.QuerySelector(@"img[src*=""button-archive""]");
            var isArchived = archiveButton is not null;

            var loggedInUserName = doc.QuerySelector("#loggedinusername")?.TextContent;
            var isLoggedIn = loggedInUserName != "Unregistered User";

            var testDiv = doc.QuerySelector(".mainbodytextlarge");
            if (testDiv is null)
            {
                throw new AwfulParserException($"ParseThread: .mainbodytextlarge");
            }

            var parentForumLinks = testDiv.QuerySelectorAll("a");
            var parentForumName = string.Empty;
            var parentForumId = 0;
            if (parentForumLinks.Length >= 1)
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

            var forumLink = testDiv.QuerySelector($"a[href*='forumid={forumId}']");
            if (forumLink is null)
            {
                throw new AwfulParserException($"ParseThread: a[href*='forumid={forumId}']");
            }

            var currentPage = 1;
            var totalPages = 1;

            var pages = doc.QuerySelector(".pages");
            if (pages is not null)
            {
                var select = pages.QuerySelector("select");
                if (select is not null)
                {
                    var selectedPageItem = select.QuerySelector("option:checked");
                    if (selectedPageItem is not null)
                    {
                        currentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
                        totalPages = select.ChildElementCount;
                    }
                }
            }

            var forumName = forumLink.TextContent;

            var scrollToPost = 0;
            var scrollToPostString = string.Empty;

            if (!string.IsNullOrEmpty(responseEndpoint))
            {
                string[] test = responseEndpoint.Split('#');

                if (test.Length > 1 && test[1].Contains("pti"))
                {
                    scrollToPost = int.Parse(Regex.Match(responseEndpoint.Split('#')[1], @"\d+").Value, CultureInfo.InvariantCulture) - 1;
                    scrollToPostString = string.Concat("#", responseEndpoint.Split('#')[1]);
                }
            }

            return new ThreadPost(
                threadId,
                title,
                new Entities.Forums.Forum(forumId, forumName, parentForumId, parentForumName),
                posts,
                currentPage,
                totalPages,
                scrollToPost,
                scrollToPostString,
                loggedInUserName,
                isLoggedIn,
                isArchived);
        }
    }
}
