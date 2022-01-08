// <copyright file="ThreadHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Awful.Core.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles SA Thread Elements.
    /// </summary>
    public static class ThreadHandler
    {
        /// <summary>
        /// Parses the IHtmlDocument for a forum thread list.
        /// </summary>
        /// <param name="doc">IHtmlDocument containing the Forum List.</param>
        /// <returns>List of Threads.</returns>
        public static List<Entities.Threads.Thread> ParseForumThreadList(IHtmlDocument doc)
        {
            PaywallHandler.CheckPaywall(doc);
            var forumThreadList = new List<Entities.Threads.Thread>();
            var threadTableList = doc.QuerySelector("#forum");
            if (threadTableList == null)
            {
                throw new AwfulParserException(Awful.Core.Resources.ExceptionMessages.ThreadListMissing);
            }

            var rows = threadTableList.QuerySelectorAll("tr");

            foreach (var row in rows)
            {
                if (row.Id is null)
                {
                    // TODO: Should this throw too?
                    continue;
                }

                var id = Convert.ToInt32(row.Id.Replace("thread", string.Empty), CultureInfo.InvariantCulture);
                var isBookmark = false;
                var hasSeen = ParseHasSeen(row.QuerySelector(".lastseen"));
                var repliesSinceLastOpened = ParseRepliesSinceLastOpened(row.QuerySelector(".lastseen"));
                var starColor = ParseStar(row.QuerySelector(".star"));
                if (!string.IsNullOrEmpty(starColor))
                {
                    isBookmark = true;
                }

                var icon = ParseIcon(row.QuerySelector(".icon"));
                var storeIcon = ParseStoreIcon(row.QuerySelector(".icon2"));
                var titleItem = row.QuerySelector(".title");
                var isSticky = ParseIsSticky(titleItem);
                var isLocked = ParseIsLocked(titleItem);
                var title = ParseTitle(titleItem);
                var authorItem = row.QuerySelector(".author");
                var author = ParseAuthor(authorItem);
                var authorId = ParseAuthorId(authorItem);
                var replies = ParseReplies(row.QuerySelector(".replies"));
                var views = ParseViews(row.QuerySelector(".views"));
                var ratingItem = row.QuerySelector(".rating");
                var ratingImage = ParseRatingImage(ratingItem);
                var totalRatingVotes = ParseTotalRatingVotes(ratingItem);
                var rating = ParseRating(ratingItem);
                var lastSeenItem = row.QuerySelector(".lastseen");
                var isAnnouncement = ParseIsAnnouncement(titleItem);
                var killedByDate = ParseKilledByDate(lastSeenItem);
                var killedBy = ParseKilledBy(lastSeenItem);
                var killedById = ParseKilledById(lastSeenItem);
                var thread = new Entities.Threads.Thread(
                    id,
                    title,
                    author,
                    authorId,
                    starColor,
                    hasSeen,
                    isLocked,
                    isBookmark,
                    isSticky,
                    isAnnouncement,
                    false,
                    hasSeen,
                    killedByDate,
                    killedBy,
                    killedById,
                    replies,
                    repliesSinceLastOpened,
                    views,
                    rating,
                    totalRatingVotes,
                    ratingImage,
                    icon,
                    storeIcon
                    );
                forumThreadList.Add(thread);
            }

            return forumThreadList;
        }

        private static DateTime ParseKilledByDate(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseKilledBy: .lastSeen");
            }

            var date = element.QuerySelector(".date");
            if (date is null)
            {
                throw new AwfulParserException($"ParseKilledBy: .lastSeen .date");
            }

            return DateTime.Parse(date.TextContent, CultureInfo.InvariantCulture);
        }

        private static long ParseKilledById(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseKilledBy: .lastSeen");
            }

            var author = element.QuerySelector(".author");
            if (author is null)
            {
                throw new AwfulParserException($"ParseKilledBy: .lastSeen .author");
            }

            return Convert.ToInt64(author.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
        }

        private static string ParseKilledBy(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseKilledBy: .lastSeen");
            }

            var author = element.QuerySelector(".author");
            if (author is null)
            {
                throw new AwfulParserException($"ParseKilledBy: .lastSeen .author");
            }

            return author.TextContent;
        }

        private static string ParseRatingImage(IElement? element)
        {
            if (element is null || element.ChildElementCount <= 0)
            {
                return string.Empty;
            }

            var img = element.QuerySelector("img");
            if (img is null)
            {
                throw new AwfulParserException($"ParseRating: img");
            }

            return img.TryGetAttribute("src");
        }

        private static int ParseTotalRatingVotes(IElement? element)
        {
            if (element is null || element.ChildElementCount <= 0)
            {
                return 0;
            }

            var img = element.QuerySelector("img");
            if (img is null)
            {
                throw new AwfulParserException($"ParseRating: img");
            }

            var firstSplit = img.TryGetAttribute("title").Split('-');
            return Convert.ToInt32(Regex.Match(firstSplit[0], @"\d+").Value, CultureInfo.InvariantCulture);
        }

        private static decimal ParseRating(IElement? element)
        {
            if (element is null || element.ChildElementCount <= 0)
            {
                return 0;
            }

            var img = element.QuerySelector("img");
            if (img is null)
            {
                throw new AwfulParserException($"ParseRating: img");
            }

            var firstSplit = img.TryGetAttribute("title").Split('-');
            return Convert.ToDecimal(Regex.Match(firstSplit[1], @"[\d]{1,4}([.,][\d]{1,2})?").Value, CultureInfo.InvariantCulture);
        }

        private static int ParseReplies(IElement? element)
        {
            if (element is null)
            {
                return 0;
            }

            return Convert.ToInt32(element.TextContent, CultureInfo.InvariantCulture);
        }

        private static int ParseViews(IElement? element)
        {
            if (element is null)
            {
                return 0;
            }

            return Convert.ToInt32(element.TextContent, CultureInfo.InvariantCulture);
        }

        private static string ParseIcon(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseIcon: .icon");
            }

            var img = element.QuerySelector("img");
            if (img == null)
            {
                throw new AwfulParserException($"ParseIcon: .icon img");
            }

            return img.TryGetAttribute("src");
        }

        private static string? ParseStoreIcon(IElement? element)
        {
            // If the element doesn't exist, it's not there.
            // We'll return null.
            if (element is null)
            {
                return null;
            }

            var img = element.QuerySelector("img");

            // If we do have an element, but can't get the image, then that's a problem.
            // We'll throw.
            if (img == null)
            {
                throw new AwfulParserException($"ParseIcon2: .icon img");
            }

            return img.TryGetAttribute("src");
        }

        private static string ParseStar(IElement? element)
        {
            if (element is null)
            {
                return string.Empty;
            }

            var starColor = element.ClassList.FirstOrDefault(n => n.Contains("bm") && !n.Contains("bm-1"));
            if (string.IsNullOrEmpty(starColor))
            {
                return string.Empty;
            }

            return starColor;
        }

        private static bool ParseHasSeen(IElement? element) => element is not null;

        private static int ParseRepliesSinceLastOpened(IElement? element)
        {
            if (element is null)
            {
                return 0;
            }

            var count = element.QuerySelector(".count");
            if (count == null)
            {
                return 0;
            }

            return Convert.ToInt32(count.TextContent, CultureInfo.InvariantCulture);
        }

        private static string ParseAuthor(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseAuthor: .author");
            }

            var authorLink = element.QuerySelector("a");
            if (authorLink is null)
            {
                throw new AwfulParserException($"ParseAuthor: .author a");
            }

            return authorLink.TextContent;
        }

        private static long ParseAuthorId(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseAuthor: .author");
            }

            var authorLink = element.QuerySelector("a");
            if (authorLink is null)
            {
                throw new AwfulParserException($"ParseAuthor: .author a");
            }

            return Convert.ToInt64(authorLink.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
        }

        private static bool ParseIsSticky(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseTitle: .title");
            }

            return element.ClassList.Contains("title_sticky");
        }

        private static bool ParseIsAnnouncement(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseTitle: .title");
            }

            return element.ClassList.Contains("announcement");
        }

        private static bool ParseIsLocked(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseTitle: .title");
            }

            return element.ClassList.Contains("arch");
        }

        private static string ParseTitle(IElement? element)
        {
            if (element is null)
            {
                throw new AwfulParserException($"ParseTitle: .title");
            }

            var threadList = element.QuerySelector(".thread_title");
            if (threadList == null)
            {
                throw new AwfulParserException($"ParseTitle: .thread_title");
            }

            return threadList.TextContent;
        }
    }
}
