// <copyright file="BanHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Html.Dom;
using Awful.Entities.Bans;
using Awful.Exceptions;

namespace Awful.Handlers
{
    /// <summary>
    /// Handles the "Banned User" pages of Something Awful.
    /// </summary>
    public static class BanHandler
    {
        /// <summary>
        /// Parses a given IHtmlDocument of the Ban user page.
        /// </summary>
        /// <param name="document">An IHtmlDocument of the ban page.</param>
        /// <returns>The BanPage.</returns>
        public static BanPage ParseBanPage(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var banList = document.QuerySelector(@"table[class=""standard full""]");
            if (banList == null)
            {
                throw new AwfulParserException($"{nameof(BanPage)}: ParseBanPage: banList");
            }

            var banPageDoc = document.QuerySelector(".pages");
            var select = banPageDoc?.QuerySelector("select");
            var selectedPageItem = select?.QuerySelector("option:checked");
            if (select == null || selectedPageItem == null)
            {
                throw new AwfulParserException($"GetPageInfo: select, selectedPageItem");
            }

            var currentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
            var totalPages = select.ChildElementCount;

            var banListBody = banList.QuerySelector("tbody");
            if (banListBody == null)
            {
                throw new AwfulParserException($"{nameof(BanPage)}: ParseBanPage: banListBody");
            }

            var banListRows = banListBody.QuerySelectorAll("tr");
            var rows = new List<BanItem>();
            foreach (var banListRow in banListRows)
            {
                var tds = banListRow.QuerySelectorAll("td");
                if (!tds.Any())
                {
                    continue;
                }

                var type = tds[0].QuerySelector("a");
                var date = tds[1];
                var horribleJerk = tds[2].QuerySelector("a");
                var reason = tds[3];
                var requestedBy = tds[4].QuerySelector("a");
                var approvedBy = tds[5].QuerySelector("a");

                if (type == null)
                {
                    continue;
                }

                if (horribleJerk == null || requestedBy == null || approvedBy == null)
                {
                    throw new AwfulParserException($"{nameof(BanPage)}: ParseBanPage: banListRow");
                }

                try
                {
                    var typeR = type.TextContent;
                    var postId = Convert.ToInt32(type.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);

                    var dateR = DateTime.Parse(date.TextContent, CultureInfo.InvariantCulture);

                    var horribleJerkR = horribleJerk.TextContent;
                    var horribleJerkIdR = Convert.ToInt32(horribleJerk.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);

                    var punishmentReason = reason.InnerHtml;

                    var requestedByR = requestedBy.TextContent;
                    var requestedById = Convert.ToInt32(requestedBy.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);

                    var approvedByR = approvedBy.TextContent;
                    var approvedById = Convert.ToInt32(approvedBy.TryGetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
                    rows.Add(new BanItem(postId, typeR, horribleJerkIdR, horribleJerkR, dateR, punishmentReason, approvedById, approvedByR, requestedById, requestedByR));
                }
                catch (Exception ex)
                {
                    throw new AwfulParserException($"{nameof(BanPage)}: ParseBanPage: banListRow", ex);
                }
            }

            return new BanPage(currentPage, totalPages, rows);
        }

        /// <summary>
        /// Parses a given IHtmlDocument of a given page to see if a user is probated.
        /// </summary>
        /// <param name="document">An IHtmlDocument of a page.</param>
        /// <returns>A ProbationItem.</returns>
        public static ProbationItem ParseForProbation(IHtmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var probationDiv = document.QuerySelector("#probation_warn");
            if (probationDiv == null)
            {
                return new ProbationItem(false);
            }

            string[] sentences = Regex.Split(probationDiv.TextContent, @"(?<=[\.!\?])\s+");
            var datestring = sentences[0].Trim().Replace("TAKE A BREAK\nYou have been put on probation until ", string.Empty).Replace(".", string.Empty);

            return new ProbationItem(true, DateTime.Parse(datestring, CultureInfo.InvariantCulture));
        }
    }
}
