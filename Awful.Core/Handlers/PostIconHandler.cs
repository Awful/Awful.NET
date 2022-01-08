// <copyright file="PostIconHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Exceptions;
using System.Globalization;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles Something Awful Post Icon Elements.
    /// </summary>
    public static class PostIconHandler
    {
        /// <summary>
        /// Parses the SA Post Icon Page.
        /// </summary>
        /// <param name="document">Document containing the Post Icon List.</param>
        /// <returns>A list of post icons.</returns>
        public static List<PostIcon> ParsePostIconList(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var postIconList = new List<PostIcon>();
            var postIconsHtml = document.QuerySelectorAll(".posticon");
            foreach (var postIconHtml in postIconsHtml)
            {
                var input = postIconHtml.QuerySelector("input");
                if (input is null)
                {
                    throw new AwfulParserException($"ParsePostIconList: input");
                }

                var inputId = Convert.ToInt32(input.TryGetAttribute("value"), CultureInfo.InvariantCulture);
                var image = postIconHtml.QuerySelector("img");

                if (image is null)
                {
                    throw new AwfulParserException($"ParsePostIconList: image");
                }

                var srcAlt = image.TryGetAttribute("alt");
                var imageEndpoint = image.TryGetAttribute("src");
                postIconList.Add(new PostIcon(inputId, imageEndpoint, srcAlt));
            }

            return postIconList;
        }
    }
}
