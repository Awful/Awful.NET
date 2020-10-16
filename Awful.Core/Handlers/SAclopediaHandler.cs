// <copyright file="SAclopediaHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.SAclopedia;

namespace Awful.Core.Handlers
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

            return document.QuerySelector(".letternav").QuerySelectorAll("a").Select(n => new SAclopediaCategory() { Letter = n.TextContent.Trim(), Id = Convert.ToInt32(n.GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture) }).ToList();
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

            return document.QuerySelector("#topiclist").QuerySelectorAll("a").Select(n => new SAclopediaEntryItem() { Title = n.TextContent.Trim(), Id = Convert.ToInt32(n.GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture) }).ToList();
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

            var saclopediaEntry = new SAclopediaEntry()
            {
                Id = id,
                Title = document.QuerySelector(@"h1[class=""topic""]").TextContent,
            };

            var postsLi = document.QuerySelector("#posts").QuerySelectorAll("li");
            foreach (var postli in postsLi)
            {
                var entry = new SAclopediaPost();
                var userThing = postli.QuerySelector(".byline");
                if (userThing != null)
                {
                    entry.Username = userThing.QuerySelector("a").TextContent;
                    entry.UserId = Convert.ToInt32(userThing.QuerySelector("a").GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
                    var htmlTest = userThing.InnerHtml;
                    var lastBracket = htmlTest.LastIndexOf('>');
                    if (lastBracket > 0)
                    {
                        entry.PostedDate = DateTime.Parse(htmlTest.Substring(lastBracket + 5), CultureInfo.InvariantCulture);
                    }
                }

                entry.PostHtml = postli.LastElementChild.InnerHtml;
                saclopediaEntry.Posts.Add(entry);
            }

            return saclopediaEntry;
        }
    }
}
