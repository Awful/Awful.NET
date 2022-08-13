// <copyright file="SmileHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;
using Awful.Entities.Smilies;
using Awful.Exceptions;

namespace Awful.Handlers
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
            if (innerList is null)
            {
                throw new AwfulParserException($"ParseSmileList: .inner");
            }

            var categoriesHeader = innerList.QuerySelectorAll("h3").Select(n => n.TextContent).ToList();
            var smileGroups = document.QuerySelectorAll(".smilie_group");
            var categoryList = new List<SmileCategory>();
            for (var i = 0; i < smileGroups.Count(); i++)
            {
                var smileGroup = smileGroups[i];
                var smileCategory = categoriesHeader[i];
                var smiles = smileGroup.QuerySelectorAll(".smilie");

                var smileList = new List<Smile>();
                foreach (var smile in smiles)
                {
                    if (smile is null)
                    {
                        throw new AwfulParserException($"ParseSmileList: smile");
                    }

                    var imageSrc = smile.QuerySelector("img");
                    if (imageSrc is null)
                    {
                        throw new AwfulParserException($"ParseSmileList: imageSrc");
                    }

                    var image = imageSrc.TryGetAttribute("src");

                    smileList.Add(new Smile(smileCategory, smile.TextContent, image));
                }

                categoryList.Add(new SmileCategory(smileCategory, smileList));
            }

            return categoryList;
        }
    }
}
