// <copyright file="CommonHandlers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using AngleSharp.Html.Dom;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Common Handlers.
    /// </summary>
    public static class CommonHandlers
    {
        /// <summary>
        /// Get the current and total pages from an element selector.
        /// </summary>
        /// <param name="doc">Document with pages selector.</param>
        /// <returns>Current Page and Total pages.</returns>
        public static (int currentPage, int totalPages) GetCurrentPageAndTotalPagesFromSelector(IHtmlDocument? doc)
        {
            if (doc is null)
            {
                return (1, 1);
            }

            var currentPage = 1;
            var totalPages = 1;

            var pages = doc.QuerySelector(".pages");
            if (pages is not null)
            {
                var select = pages.QuerySelector("select");
                if (select is not null)
                {
                    var selectedPageItem = select.QuerySelector("option:checked");
                    if (selectedPageItem is not null)
                    {
                        currentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
                        totalPages = select.ChildElementCount;
                    }
                }
            }

            return (currentPage, totalPages);
        }
    }
}
