// <copyright file="UserHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Awful.Entities.Posts;
using Awful.Exceptions;

namespace Awful.Handlers
{
    /// <summary>
    /// User Handler.
    /// </summary>
    public static class UserHandler
    {
        /// <summary>
        /// Parses the User Document from their profile page.
        /// </summary>
        /// <param name="doc">Document containing the User Profile.</param>
        /// <returns>A user.</returns>
        public static User ParseUserFromProfilePage(IHtmlDocument doc)
        {
            var authorTd = doc.QuerySelector(@"[class*=""userinfo""]");
            if (authorTd == null)
            {
                throw new AwfulParserException($"{nameof(User)}: ParseUserFromProfilePage: authorTd");
            }

            return ParseUserInfoElement(authorTd);
        }

        /// <summary>
        /// Parses a user profile from a post.
        /// </summary>
        /// <param name="doc">The user element from a post.</param>
        /// <returns>A user.</returns>
        public static User ParseUserFromPost(IElement doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var authorTd = doc.QuerySelector(@"[class*=""userid""]");
            if (authorTd == null)
            {
                throw new AwfulParserException($"{nameof(User)}: ParseUserFromPost: authorTd");
            }

            return ParseUserInfoElement(authorTd);
        }

        /// <summary>
        /// Parse User Info from Element.
        /// </summary>
        /// <param name="authorTd">Author TD.</param>
        /// <returns><see cref="User"/>.</returns>
        /// <exception cref="AwfulParserException">Thrown if failed to parse element.</exception>
        public static User ParseUserInfoElement(IElement authorTd)
        {
            var userId = authorTd.ClassList.FirstOrDefault(n => n.Contains("userid-"))?.Trim().Replace("userid-", string.Empty);
            if (userId is null)
            {
                throw new AwfulParserException($"{nameof(User)}: ParseUserInfoElement: userid");
            }

            var id = Convert.ToInt64(userId, CultureInfo.InvariantCulture);

            var authorTitle = authorTd.QuerySelector(".author");
            if (authorTitle?.ClassName == null)
            {
                throw new AwfulParserException($"{nameof(User)}: ParseUserInfoElement: authorTitle");
            }

            var username = authorTitle.TextContent;
            if (string.IsNullOrEmpty(username))
            {
                throw new AwfulParserException($"{nameof(User)}: ParseUserInfoElement: authorTitle");
            }

            var roles = authorTitle.ClassName;
            var title = authorTitle.TryGetAttribute("title");
            var userTitleHtml = authorTd.QuerySelector(".title");

            var registered = authorTd.QuerySelector(".registered");

            DateTime? dateJoined = null;
            if (registered != null)
            {
                dateJoined = DateTime.Parse(registered.TextContent, CultureInfo.InvariantCulture);
            }

            string? avatarHtml = null;
            string? avatarTitle = null;
            string? avatarLink = null;
            string? avatarGangTagLink = null;

            if (userTitleHtml is not null)
            {
                avatarHtml = userTitleHtml.InnerHtml;
                avatarTitle = userTitleHtml.TextContent.Trim();

                var userImgs = userTitleHtml.QuerySelectorAll(@"img");
                if (userImgs != null && userImgs.Any())
                {
                    avatarLink = userImgs.First().TryGetAttribute("src");
                }

                if (avatarLink == null)
                {
                    var userImgAv = userTitleHtml.QuerySelector(@"img[src*=""titles""]");
                    if (userImgAv != null)
                    {
                        avatarLink = userImgAv.TryGetAttribute("src");
                    }
                }

                var gangTagImg = userTitleHtml.QuerySelector(@"img[src*=""gangtags""]");
                if (gangTagImg != null)
                {
                    avatarGangTagLink = gangTagImg.TryGetAttribute("src");
                }
            }

            return new User(id, username, dateJoined, title, roles, avatarTitle, avatarHtml, avatarLink, avatarGangTagLink);
        }
    }
}
