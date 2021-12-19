// <copyright file="SmileHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Smilies;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles Something Awful Smile Elements.
    /// </summary>
    public static class SmileHandler
    {
        /// <summary>
        /// Parses the SA Smile Page Document.
        /// </summary>
        /// <param name="document">The SA Smile Page.</param>
        /// <returns>A list of Smile Categories, containing the list of smilies.</returns>
        public static List<SmileCategory> ParseSmileList(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var innerList = document.QuerySelector(".inner");
            var categoriesHeader = innerList.QuerySelectorAll("h3").Select(n => new SmileCategory() { Name = n.TextContent }).ToList();
            var smileGroups = document.QuerySelectorAll(".smilie_group");
            for (var i = 0; i < smileGroups.Count(); i++)
            {
                var smileGroup = smileGroups[i];
                var smileCategory = categoriesHeader[i];
                var smiles = smileGroup.QuerySelectorAll(".smilie");
                foreach (var smile in smiles)
                {
                    var image = smile.QuerySelector("img").TryGetAttribute("src");
                    smileCategory.SmileList.Add(new Smile()
                    {
                        Category = smileCategory.Name,
                        Title = smile.TextContent.Trim(),
                        ImageEndpoint = image,
                        ImageLocation = Path.GetFileNameWithoutExtension(image),
                    });
                }
            }

            return categoriesHeader;
        }
    }
}
