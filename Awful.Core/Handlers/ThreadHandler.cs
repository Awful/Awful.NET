// <copyright file="ThreadHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Threads;
using Awful.Core.Exceptions;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles SA Thread Elements.
    /// </summary>
    public static class ThreadHandler
    {
        /// <summary>
        /// Checks if the current IHtmlDocument contains a paywall message.
        /// Throws a PaywallException if the content is paywalled.
        /// </summary>
        /// <param name="doc">IHtmlDocument containing the SA Page.</param>
        public static void CheckPaywall(IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var test = doc.QuerySelector(".inner");
            if (test != null)
            {
                if (test.TextContent.Contains("Sorry, you must be a registered forums member to view this page."))
                {
                    throw new PaywallException(Awful.Core.Resources.ExceptionMessages.PaywallThreadHit);
                }
            }
        }

        /// <summary>
        /// Parses the IHtmlDocument for a forum thread list.
        /// </summary>
        /// <param name="doc">IHtmlDocument containing the Forum List.</param>
        /// <returns>List of Threads.</returns>
        public static List<Thread> ParseForumThreadList(IHtmlDocument doc)
        {
            CheckPaywall(doc);
            var forumThreadList = new List<Thread>();
            var threadTableList = doc.QuerySelector("#forum");
            if (threadTableList == null)
            {
                throw new FormatException(Awful.Core.Resources.ExceptionMessages.ThreadListMissing);
            }

            var rows = threadTableList.QuerySelectorAll("tr");

            foreach (var row in rows)
            {
                if (row.Id == null)
                {
                    continue;
                }

                var thread = new Thread
                {
                    ThreadId = Convert.ToInt32(row.Id.Replace("thread", string.Empty), CultureInfo.InvariantCulture),
                };
                ParseStar(row.QuerySelector(".star"), thread);
                ParseIcon(row.QuerySelector(".icon"), thread);
                ParseIcon2(row.QuerySelector(".icon2"), thread);
                ParseTitle(row.QuerySelector(".title"), thread);
                ParseAuthor(row.QuerySelector(".author"), thread);
                ParseReplies(row.QuerySelector(".replies"), thread);
                ParseViews(row.QuerySelector(".views"), thread);
                ParseRating(row.QuerySelector(".rating"), thread);
                ParseLastPost(row.QuerySelector(".lastpost"), thread);
                ParseLastSeen(row.QuerySelector(".lastseen"), thread);
                forumThreadList.Add(thread);
            }

            return forumThreadList;
        }

        /// <summary>
        /// Parses the content of a new thread page to get info about it.
        /// </summary>
        /// <param name="document">The IHtmlDocument of the New Thread page.</param>
        /// <returns>NewThread.</returns>
        public static NewThread ParseNewThread(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return new NewThread
            {
                FormKey = document.QuerySelector(@"input[name=""formkey""]").TryGetAttribute("value"),
                FormCookie = document.QuerySelector(@"input[name=""form_cookie""]").TryGetAttribute("value"),
            };
        }

        private static void ParseStar(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            var starColor = element.ClassList.FirstOrDefault(n => n.Contains("bm") && !n.Contains("bm-1"));
            if (string.IsNullOrEmpty(starColor))
            {
                return;
            }

            thread.StarColor = starColor;
            thread.IsBookmark = true;
        }

        private static void ParseLastSeen(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            thread.HasSeen = true;
            var count = element.QuerySelector(".count");
            if (count == null)
            {
                return;
            }

            thread.RepliesSinceLastOpened = Convert.ToInt32(count.TextContent, CultureInfo.InvariantCulture);
        }

        private static void ParseIcon(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            var img = element.QuerySelector("img");
            thread.ImageIconEndpoint = img.TryGetAttribute("src");
            thread.ImageIconLocation = Path.GetFileNameWithoutExtension(thread.ImageIconEndpoint);
        }

        private static void ParseIcon2(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            var img = element.QuerySelector("img");
            thread.StoreImageIconEndpoint = img.TryGetAttribute("src");
            thread.StoreImageIconLocation = Path.GetFileNameWithoutExtension(thread.StoreImageIconEndpoint);
        }

        private static void ParseTitle(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            if (element.ClassList.Contains("title_sticky"))
            {
                thread.IsSticky = true;
            }

            var threadList = element.QuerySelector(".thread_title");
            thread.Name = threadList.TextContent;
        }

        private static void ParseAuthor(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            var authorLink = element.QuerySelector("a");
            thread.Author = authorLink.TextContent;
            thread.AuthorId = Convert.ToInt64(authorLink.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
        }

        private static void ParseReplies(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            thread.ReplyCount = Convert.ToInt32(element.TextContent, CultureInfo.InvariantCulture);
            thread.TotalPages = (thread.ReplyCount / 40) + 1;
        }

        private static void ParseViews(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            thread.ViewCount = Convert.ToInt32(element.TextContent, CultureInfo.InvariantCulture);
        }

        private static void ParseRating(IElement element, Thread thread)
        {
            if (element == null || element.ChildElementCount <= 0)
            {
                return;
            }

            var img = element.QuerySelector("img");
            thread.RatingImageEndpoint = img.TryGetAttribute("src");
            thread.RatingImage = Path.GetFileNameWithoutExtension(thread.RatingImageEndpoint);
            var firstSplit = img.TryGetAttribute("title").Split('-');
            thread.TotalRatingVotes = Convert.ToInt32(Regex.Match(firstSplit[0], @"\d+").Value, CultureInfo.InvariantCulture);
            thread.Rating = Convert.ToDecimal(Regex.Match(firstSplit[1], @"[\d]{1,4}([.,][\d]{1,2})?").Value, CultureInfo.InvariantCulture);
        }

        private static void ParseLastPost(IElement element, Thread thread)
        {
            if (element == null)
            {
                return;
            }

            var date = element.QuerySelector(".date");
            var author = element.QuerySelector(".author");
            thread.KilledOn = DateTime.Parse(date.TextContent, CultureInfo.InvariantCulture);
            thread.KilledById = Convert.ToInt64(author.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
            thread.KilledBy = author.TextContent;
        }
    }
}
