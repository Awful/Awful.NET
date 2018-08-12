using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Awful.Parser.Core;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Users;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Messages;
using Awful.Parser.Models.PostIcons;

namespace Awful.Parser.Handlers
{
    public class PrivateMessageHandler
    {
        public static List<PrivateMessage> ParseList(IHtmlDocument doc)
        {
            var privateMessageList = new List<PrivateMessage>();

            var pmList = doc.QuerySelector(".standard.full");
            if (pmList == null)
                return privateMessageList;

            var pmListBody = doc.QuerySelector("tbody");
            var pmListRows = pmListBody.QuerySelectorAll("tr");
            foreach(var pmRow in pmListRows)
            {
                privateMessageList.Add(ParseRow(pmRow));
            }

            return privateMessageList;
        }

        private static PrivateMessage ParseRow(IElement elment)
        {
            var pm = new PrivateMessage();
            ParseStatus(elment.QuerySelector(".status"), pm);
            ParseIcon(elment.QuerySelector(".icon"), pm);
            ParseTitle(elment.QuerySelector(".title"), pm);
            ParseSender(elment.QuerySelector(".sender"), pm);
            ParseDate(elment.QuerySelector(".date"), pm);
            return pm;
        }

        private static void ParseStatus(IElement element, PrivateMessage pm)
        {
            if (element == null)
                return;
            var img = element.QuerySelector("img");
            pm.StatusImageIconUrl = img.GetAttribute("src");
            pm.StatusImageIconLocation = Path.GetFileNameWithoutExtension(pm.ImageIconUrl);
        }

        private static void ParseIcon(IElement element, PrivateMessage pm)
        {
            if (element == null)
                return;
            var img = element.QuerySelector("img");
            if (img == null)
                return;
            pm.ImageIconUrl = img.GetAttribute("src");
            pm.ImageIconLocation = Path.GetFileNameWithoutExtension(pm.ImageIconUrl);
            pm.Icon = new PostIcon() { ImageUrl = pm.ImageIconUrl };
        }

        private static void ParseTitle(IElement element, PrivateMessage pm)
        {
            if (element == null)
                return;
            var threadList = element.QuerySelector("a");
            pm.MessageUrl = threadList.GetAttribute("href");
            pm.Id = Convert.ToInt32(pm.MessageUrl.Split('=').Last());
            pm.Title = threadList.TextContent;
        }

        private static void ParseSender(IElement element, PrivateMessage pm)
        {
            if (element == null)
                return;
            pm.Sender = element.TextContent;
        }

        private static void ParseDate(IElement element, PrivateMessage pm)
        {
            if (element == null)
                return;
            pm.Date = DateTime.Parse(element.TextContent.Replace("at", ""));
        }
    }
}
