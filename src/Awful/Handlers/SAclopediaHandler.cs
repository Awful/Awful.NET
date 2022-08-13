// <copyright file="SAclopediaHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using AngleSharp.Html.Dom;
using Awful.Entities.SAclopedia;
using Awful.Exceptions;

namespace Awful.Handlers
{
    /// <summary>
    /// Handles SAclopedia Elements.
    /// </summary>
    public static class SAclopediaHandler
    {
        /// <summary>
        /// Parse an SAclopedia category list.
        /// </summary>
        /// <param name="document">The SAclopedia Document.</param>
        /// <returns>List of SAclopedia categories.</returns>
        public static List<SAclopediaCategory> ParseCategoryList(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var letterNav = document.GetElementViaQuerySelector(".letternav");
            var links = letterNav?.QuerySelectorAll("a");
            if (links is null)
            {
                throw new AwfulParserException($"ParseCategoryList: letternav");
            }

            return links.Select(n => new SAclopediaCategory(Convert.ToInt32(n.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture), n.TextContent.Trim())).ToList();
        }

        /// <summary>
        /// Parse an SAclopedia Entry list.
        /// </summary>
        /// <param name="document">The SAclopedia Document.</param>
        /// <returns>List of SAclopedia Entries.</returns>
        public static List<SAclopediaEntryItem> ParseEntryItemList(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var topicList = document.GetElementViaQuerySelector("#topiclist");
            var links = topicList?.QuerySelectorAll("a");
            if (links is null)
            {
                throw new AwfulParserException($"ParseEntryItemList: #topiclist");
            }

            return links.Select(n => new SAclopediaEntryItem(Convert.ToInt32(n.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture), n.TextContent.Trim())).ToList();
        }

        /// <summary>
        /// Parse an SAclopedia entry.
        /// </summary>
        /// <param name="document">The SAclopedia Document.</param>
        /// <param name="id">The SAclopedia Id.</param>
        /// <returns>The SAclopedia Entry.</returns>
        public static SAclopediaEntry ParseEntry(IHtmlDocument document, int id)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var title = document.GetElementViaQuerySelector(@"h1[class=""topic""]");
            if (title is null)
            {
                throw new AwfulParserException($"ParseEntry: h1[class=topic]");
            }

            var postsLi = document.GetElementViaQuerySelector("#posts")?.QuerySelectorAll("li");

            if (postsLi is null)
            {
                throw new AwfulParserException($"ParseEntry: posts");
            }

            var list = new List<SAclopediaPost>();

            foreach (var postli in postsLi)
            {
                var userThing = postli.QuerySelector(".byline");
                if (userThing is null)
                {
                    throw new AwfulParserException($"ParseEntry: byline");
                }

                var userThingLink = userThing.GetElementViaQuerySelector("a");
                if (userThingLink is null)
                {
                    throw new AwfulParserException($"ParseEntry: byline a");
                }

                var username = userThingLink.TextContent;
                var userId = Convert.ToInt32(userThingLink.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
                var htmlTest = userThing.InnerHtml;
                var lastBracket = htmlTest.LastIndexOf('>');
                var postedDate = DateTime.MinValue;
                if (lastBracket > 0)
                {
                    postedDate = DateTime.Parse(htmlTest.Substring(lastBracket + 5), CultureInfo.InvariantCulture);
                }

                if (postli?.LastElementChild == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find postli while parsing SAclopediaEntry.");
                }

                var postHtml = postli.LastElementChild.InnerHtml;
                list.Add(new SAclopediaPost(userId, username, postHtml, postedDate));
            }

            return new SAclopediaEntry(id, title.TextContent, list);
        }
    }
}
