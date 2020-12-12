// <copyright file="BanHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Bans;

namespace Awful.Parser.Handlers
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

            var banPage = new BanPage();
            var banList = document.QuerySelector(@"table[class=""standard full""]");
            if (banList == null)
            {
                return banPage;
            }

            GetPageInfo(document, banPage);

            var banListBody = banList.QuerySelector("tbody");
            var banListRows = banListBody.QuerySelectorAll("tr");
            foreach (var banListRow in banListRows)
            {
                var banItem = new BanItem();
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

                banItem.Type = type.TextContent;
                banItem.PostId = Convert.ToInt32(type.GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);

                banItem.Date = DateTime.Parse(date.TextContent, CultureInfo.InvariantCulture);

                banItem.HorribleJerk = horribleJerk.TextContent;
                banItem.HorribleJerkId = Convert.ToInt32(horribleJerk.GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);

                banItem.PunishmentReason = reason.InnerHtml;

                banItem.RequestedBy = requestedBy.TextContent;
                banItem.RequestedById = Convert.ToInt32(requestedBy.GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);

                banItem.ApprovedBy = approvedBy.TextContent;
                banItem.ApprovedById = Convert.ToInt32(approvedBy.GetAttribute("href").Split('=').Last(), CultureInfo.InvariantCulture);
                banPage.Bans.Add(banItem);
            }

            return banPage;
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

            var probationItem = new ProbationItem();

            var probationDiv = document.QuerySelector("#probation_warn");
            if (probationDiv == null)
            {
                return probationItem;
            }

            probationItem.IsUnderProbation = true;
            string[] sentences = Regex.Split(probationDiv.TextContent, @"(?<=[\.!\?])\s+");
            var datestring = sentences[0].Trim().Replace("TAKE A BREAK\nYou have been put on probation until ", string.Empty).Replace(".", string.Empty);
            probationItem.ProbationUntil = DateTime.Parse(datestring, CultureInfo.InvariantCulture);

            return probationItem;
        }

        private static void GetPageInfo(IHtmlDocument doc, BanPage banPage)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            if (banPage == null)
            {
                throw new ArgumentNullException(nameof(banPage));
            }

            var banPageDoc = doc.QuerySelector(".pages");
            var select = banPageDoc.QuerySelector("select");
            var selectedPageItem = select.QuerySelector("option:checked");
            banPage.CurrentPage = Convert.ToInt32(selectedPageItem.TextContent, CultureInfo.InvariantCulture);
            banPage.TotalPages = select.ChildElementCount;
        }
    }
}
