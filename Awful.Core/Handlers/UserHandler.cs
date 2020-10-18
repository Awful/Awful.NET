// <copyright file="UserHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Posts;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles Something Awful User Elements.
    /// </summary>
    public static class UserHandler
    {
        /// <summary>
        /// Parses the User Document from their profile page.
        /// </summary>
        /// <param name="userId">The user id. 0 returns the current user if its their current page.</param>
        /// <param name="doc">Document containing the User Profile.</param>
        /// <returns>A user.</returns>
        public static User ParseUserFromProfilePage(long userId, IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var user = new User();
            user.Id = userId;

            var authorTd = doc.QuerySelector(@"[class*=""userinfo""]");
            ParseUserInfoElement(user, authorTd);

            // If we're trying to get the current user,
            // get the real ID from the page.
            if (userId == 0)
            {
                ParseUserId(user, doc);
            }

            return user;
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

            var user = new User();

            var authorTd = doc.QuerySelector(@"[class*=""userid""]");

            var userId = authorTd.ClassList.First(n => n.Contains("userid-")).Trim().Replace("userid-", string.Empty);
            user.Id = Convert.ToInt64(userId, CultureInfo.InvariantCulture);

            ParseUserInfoElement(user, authorTd);

            return user;
        }

        private static void ParseUserId(User user, IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var input = doc.QuerySelector(@"input[name=""userid""]");
            if (input != null)
            {
                var inputNum = input.GetAttribute("value");
                if (!string.IsNullOrEmpty(inputNum))
                {
                    user.Id = Convert.ToInt64(inputNum, CultureInfo.InvariantCulture);
                }
            }
        }

        private static void ParseUserInfoElement(User user, IElement authorTd)
        {
            var authorTitle = authorTd.QuerySelector(".author");
            user.Username = authorTitle.TextContent;
            user.Roles = authorTitle.ClassName;
            user.Title = authorTitle.GetAttribute("title");
            var userTitleHtml = authorTd.QuerySelector(".title");

            var registered = authorTd.QuerySelector(".registered");

            if (registered != null)
            {
                user.DateJoined = DateTime.Parse(registered.TextContent, CultureInfo.InvariantCulture);
            }

            if (userTitleHtml == null)
            {
                return;
            }

            user.AvatarHtml = userTitleHtml.InnerHtml;
            user.AvatarTitle = userTitleHtml.TextContent.Trim();

            var userImgs = userTitleHtml.QuerySelectorAll(@"img");
            if (userImgs != null && userImgs.Any())
            {
                user.AvatarLink = userImgs.First().GetAttribute("src");
            }

            if (user.AvatarLink == null)
            {
                var userImgAv = userTitleHtml.QuerySelector(@"img[src*=""titles""]");
                if (userImgAv != null)
                {
                    user.AvatarLink = userImgAv.GetAttribute("src");
                }
            }

            var gangTagImg = userTitleHtml.QuerySelector(@"img[src*=""gangtags""]");
            if (gangTagImg != null)
            {
                user.AvatarGangTagLink = gangTagImg.GetAttribute("src");
            }
        }
    }
}
