// <copyright file="PostHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Posts;
using Awful.Core.Exceptions;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles Something Awful Post Elements.
    /// </summary>
    public static class PostHandler
    {
        /// <summary>
        /// Parses an SA Posts IElement.
        /// </summary>
        /// <param name="parent">The posts parent document.</param>
        /// <param name="doc">The post's element.</param>
        /// <param name="wrapImgurGifs">Scans and wraps a posts Imgur embedded GIFs with lighter weight placeholders.</param>
        /// <returns>An SA Post.</returns>
        public static Post ParsePost(IHtmlDocument parent, IElement doc, bool wrapImgurGifs = true)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var user = UserHandler.ParseUserFromPost(doc);
            var id = doc.Id?.Replace("post", string.Empty);
            if (id is null)
            {
                throw new AwfulParserException($"{nameof(Post)}: ParsePost: id");
            }

            long idVal = 0;
            long.TryParse(id, out idVal);
            var postId = idVal;

            var index = doc.TryGetAttribute("data-idx");
            long indexVal = 0;
            long.TryParse(index, out indexVal);
            var postIndex = indexVal;

            var authorTd = doc.QuerySelector(@"[class*=""userid""]");
            authorTd?.Remove();

            var hasSeen = doc.QuerySelector(@"[class=""seen1""]") != null || doc.QuerySelector(@"[class=""seen2""]") != null;
            var isIgnored = false;
            var postDate = doc.QuerySelector(@"td[class=""postdate""]");
            var postDateText = string.Empty;
            DateTime postDateTime = default;
            if (postDate != null)
            {
                postDateText = postDate.Text().Replace("\n", string.Empty).Replace("#", string.Empty).Replace("?", string.Empty);
                DateTime.TryParse(postDateText, out var postDateTime2);
                if (postDateTime2 == default)
                {
                    throw new AwfulParserException($"{nameof(Post)}: ParsePost: postDateTime2");
                }

                postDateTime = postDateTime2;
            }

            string? postHtml = null;
            var threadBody = doc.QuerySelector(".postbody");
            if (threadBody != null)
            {
                var jerkBody = threadBody.QuerySelector(@"a[title=""DON'T DO IT!!""]");
                if (jerkBody != null)
                {
                    isIgnored = true;
                }
                else
                {
                    if (wrapImgurGifs)
                    {
                        var imgurGifs = threadBody.QuerySelectorAll(@"[src*=""imgur.com""][src*="".gif""]");
                        for (var i = 0; i < imgurGifs.Length; i++)
                        {
                            var imgurGif = imgurGifs[i];
                            var div = parent.CreateElement("div");
                            div.ClassList.Add("gif-wrap");
                            var newImgur = parent.CreateElement("img");
                            newImgur.ClassList.Add("posterized");
                            newImgur.SetAttribute("data-original-url", imgurGif.TryGetAttribute("src"));
                            newImgur.SetAttribute("data-poster-url", imgurGif.TryGetAttribute("src").Replace(".gif", "h.jpg"));
                            newImgur.SetAttribute("src", imgurGif.TryGetAttribute("src").Replace(".gif", "h.jpg"));
                            div.AppendChild(newImgur);
                            imgurGif.Replace(div);
                        }
                    }

                    var tweets = threadBody.QuerySelectorAll(@"a[href*=""twitter.com""]");
                    foreach (var tweet in tweets)
                    {
                        var href = tweet.TryGetAttribute("href");
                        var captures = Regex.Match(href, @"^https?:\/\/twitter\.com\/(?:#!\/)?(\w+)\/status(?:es)?\/(\d+)(?:\/.*)?$");
                        if (captures.Success && captures.Groups.Count >= 3)
                        {
                            tweet.SetAttribute("data-tweet-id", captures.Groups[2].Value);
                        }
                    }

                    var attachments = threadBody.QuerySelectorAll(@"[src*=""attachment.php""]");
                    foreach (var attachment in attachments)
                    {
                        var attachmentValue = attachment.Attributes["src"]?.Value;
                        if (attachmentValue is not null)
                        {
                            attachment.SetAttribute("src", $"https://forums.somethingawful.com/{attachmentValue}");
                        }
                    }
                }

                postHtml = HtmlEncode(threadBody.InnerHtml);
            }

            if (postHtml is null)
            {
                throw new AwfulParserException($"{nameof(Post)}: ParsePost: postHtml");
            }

            if (postDateTime == default)
            {
                throw new AwfulParserException($"{nameof(Post)}: ParsePost: postDateTime");
            }

            return new Post(postId, postHtml, postDateTime, user, postIndex, hasSeen, isIgnored);
        }

        /// <summary>
        /// Parses a SA post preview element.
        /// </summary>
        /// <param name="doc">SA Post IHtmlDocument.</param>
        /// <returns>An SA Post.</returns>
        public static string ParsePostPreview(IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var threadBody = doc.QuerySelector(".postbody");
            return threadBody?.OuterHtml ?? string.Empty;
        }

        /// <summary>
        /// Parses the previous posts shown when making a new post.
        /// </summary>
        /// <param name="doc">The IHtmlDocument of a new post page.</param>
        /// <returns>List of Posts.</returns>
        public static List<Post> ParsePreviousPosts(IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var posts = new List<Post>();
            var threadDivTableHolder = doc.QuerySelector("#thread");
            if (threadDivTableHolder is null)
            {
                throw new AwfulParserException($"{nameof(Post)}: ParsePreviousPosts: threadDivTableHolder");
            }

            foreach (var threadTable in threadDivTableHolder.QuerySelectorAll("table"))
            {
                if (string.IsNullOrEmpty(threadTable.TryGetAttribute("data-idx")))
                {
                    continue;
                }

                posts.Add(PostHandler.ParsePost(doc, threadTable));
            }

            return posts;
        }

        /// <summary>
        /// Encodes a SA Posts HTML.
        /// In order to get Unicode characters fully working, we need to first encode the entire post.
        /// THEN we decode the bits we can safely pass in, like single/double quotes.
        /// If we don't, the post format will be screwed up.
        /// </summary>
        /// <param name="text">The posts text.</param>
        /// <returns>An SA HTML encoded string.</returns>
        private static string HtmlEncode(string text)
        {
            char[] chars = WebUtility.HtmlEncode(text).ToCharArray();
            var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, "&#{0};", value);
                }
                else
                {
                    result.Append(c);
                }
            }

            result.Replace("&quot;", "\"");
            result.Replace("&#39;", @"'");
            result.Replace("&lt;", @"<");
            result.Replace("&gt;", @">");
            return result.ToString();
        }
    }
}
