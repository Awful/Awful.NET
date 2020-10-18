// <copyright file="ThreadPostHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Threads;
using Awful.Core.Exceptions;

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
            var thread = new ThreadPost();

            CheckPaywall(doc);
            ParseArchive(doc, thread);
            ParseThreadInfo(doc, thread, responseEndpoint);
            ParseThreadPageNumbers(doc, thread);
            ParseThreadPage(doc, thread);
            ParseThreadPosts(doc, thread);
            return thread;
        }

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

        private static void ParseThreadPage(IHtmlDocument doc, ThreadPost thread)
        {
            thread.Name = doc.Title.Replace(" - The Something Awful Forums", string.Empty);
            var threadBody = doc.QuerySelector("body");
            thread.ThreadId = Convert.ToInt32(threadBody.GetAttribute("data-thread"), CultureInfo.InvariantCulture);
            thread.ForumId = Convert.ToInt32(threadBody.GetAttribute("data-forum"), CultureInfo.InvariantCulture);
        }

        private static void ParseThreadPosts(IHtmlDocument doc, ThreadPost thread)
        {
            var threadDivTableHolder = doc.QuerySelector("#thread");
            foreach (var threadTable in threadDivTableHolder.QuerySelectorAll("table.post"))
            {
                if (string.IsNullOrEmpty(threadTable.Id.Replace("post", string.Empty)))
                {
                    continue;
                }

                thread.Posts.Add(PostHandler.ParsePost(doc, threadTable));
            }
        }

        private static void ParseArchive(IHtmlDocument doc, ThreadPost thread)
        {
            var archiveButton = doc.QuerySelector(@"img[src*=""button-archive""]");
            if (archiveButton != null)
            {
                thread.IsArchived = true;
            }
        }

        private static void ParseThreadInfo(IHtmlDocument doc, ThreadPost thread, string responseUri = "")
        {
            thread.LoggedInUserName = doc.QuerySelector("#loggedinusername").TextContent;
            thread.IsLoggedIn = thread.LoggedInUserName != "Unregistered Faggot";
            if (string.IsNullOrEmpty(responseUri))
            {
                return;
            }

            string[] test = responseUri.Split('#');

            if (test.Length > 1 && test[1].Contains("pti"))
            {
                thread.ScrollToPost = int.Parse(Regex.Match(responseUri.Split('#')[1], @"\d+").Value, CultureInfo.InvariantCulture) - 1;
                thread.ScrollToPostString = string.Concat("#", responseUri.Split('#')[1]);
            }
        }

        private static void ParseThreadPageNumbers(IHtmlDocument doc, ThreadPost thread)
        {
            var pages = doc.QuerySelector(".pages");
            if (pages == null)
            {
                thread.CurrentPage = 1;
                thread.TotalPages = 1;
                return;
            }

            var select = pages.QuerySelector("select");
            if (select == null)
            {
                thread.CurrentPage = 1;
                thread.TotalPages = 1;
                return;
            }

            var selectedPageItem = select.QuerySelector("option:checked");
            thread.CurrentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
            thread.TotalPages = select.ChildElementCount;
        }
    }
}
