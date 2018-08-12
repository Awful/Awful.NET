using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Awful.Parser.Core;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Bans;
using System.Text.RegularExpressions;

namespace Awful.Parser.Handlers
{
    public class BanHandler
    {
        public static BanPage ParseBanPage(IHtmlDocument document)
        {
            var banPage = new BanPage();
            var banList = document.QuerySelector(@"table[class=""standard full""]");
            if (banList == null)
                return banPage;

            GetPageInfo(document, banPage);

            var banListBody = banList.QuerySelector("tbody");
            var banListRows = banListBody.QuerySelectorAll("tr");
            foreach(var banListRow in banListRows)
            {
                var banItem = new BanItem();
                var tds = banListRow.QuerySelectorAll("td");
                if (!tds.Any())
                    continue;
                var type = tds[0].QuerySelector("a");
                var date = tds[1];
                var horribleJerk = tds[2].QuerySelector("a");
                var reason = tds[3];
                var requestedBy = tds[4].QuerySelector("a");
                var approvedBy = tds[5].QuerySelector("a");

                banItem.Type = type.TextContent;
                banItem.PostId = Convert.ToInt32(type.GetAttribute("href").Split('=').Last());

                banItem.Date = DateTime.Parse(date.TextContent);

                banItem.HorribleJerk = horribleJerk.TextContent;
                banItem.HorribleJerkId = Convert.ToInt32(horribleJerk.GetAttribute("href").Split('=').Last());

                banItem.PunishmentReason = reason.InnerHtml;

                banItem.RequestedBy = requestedBy.TextContent;
                banItem.RequestedById = Convert.ToInt32(requestedBy.GetAttribute("href").Split('=').Last());

                banItem.ApprovedBy = approvedBy.TextContent;
                banItem.ApprovedById = Convert.ToInt32(approvedBy.GetAttribute("href").Split('=').Last());
                banPage.Bans.Add(banItem);
            }

            return banPage;
        }

        public static ProbationItem ParseForProbation(IHtmlDocument document)
        {
            var probationItem = new ProbationItem();

            var probationDiv = document.QuerySelector("#probation_warn");
            if (probationDiv == null)
                return probationItem;

            probationItem.IsUnderProbation = true;
            string[] sentences = Regex.Split(probationDiv.TextContent, @"(?<=[\.!\?])\s+");
            var datestring = sentences[1].Replace("You have been put on probation until ","").Replace("CST.", "");
            probationItem.ProbationUntil = DateTime.Parse(datestring);

            return probationItem;
        }


        private static void GetPageInfo(IHtmlDocument doc, BanPage banPage)
        {
            var banPageDoc = doc.QuerySelector(".pages");
            if (banPage == null)
                return;
            var select = banPageDoc.QuerySelector("select");
            var selectedPageItem = select.QuerySelector("option:checked");
            banPage.CurrentPage = Convert.ToInt32(selectedPageItem.TextContent);
            banPage.TotalPages = select.ChildElementCount;
        }
    }
}
