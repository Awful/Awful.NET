// <copyright file="PrivateMessageHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.PostIcons;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Handles Something Awful Private Message Elements.
    /// </summary>
    public static class PrivateMessageHandler
    {
        /// <summary>
        /// Parses the SA Private Message page list for messages.
        /// </summary>
        /// <param name="doc">Document containing the SA PM page.</param>
        /// <returns>List of Private Messages.</returns>
        public static List<PrivateMessage> ParseList(IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var privateMessageList = new List<PrivateMessage>();

            var pmList = doc.QuerySelector(".standard.full");
            if (pmList == null)
            {
                return privateMessageList;
            }

            var pmListBody = doc.QuerySelector("tbody");
            var pmListRows = pmListBody.QuerySelectorAll("tr");
            foreach (var pmRow in pmListRows)
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
            {
                return;
            }

            var img = element.QuerySelector("img");
            pm.StatusImageIconEndpoint = img.GetAttribute("src");
            pm.StatusImageIconLocation = Path.GetFileNameWithoutExtension(pm.ImageIconEndpoint);
        }

        private static void ParseIcon(IElement element, PrivateMessage pm)
        {
            if (element == null)
            {
                return;
            }

            var img = element.QuerySelector("img");
            if (img == null)
            {
                return;
            }

            pm.ImageIconEndpoint = img.GetAttribute("src");
            pm.ImageIconLocation = Path.GetFileNameWithoutExtension(pm.ImageIconEndpoint);
            pm.Icon = new PostIcon() { ImageEndpoint = pm.ImageIconEndpoint };
        }

        private static void ParseTitle(IElement element, PrivateMessage pm)
        {
            if (element == null)
            {
                return;
            }

            var threadList = element.QuerySelector("a");
            pm.MessageEndpoint = threadList.GetAttribute("href");
            pm.Id = Convert.ToInt32(pm.MessageEndpoint.Split('=').Last(), CultureInfo.InvariantCulture);
            pm.Title = threadList.TextContent;
        }

        private static void ParseSender(IElement element, PrivateMessage pm)
        {
            if (element == null)
            {
                return;
            }

            pm.Sender = element.TextContent;
        }

        private static void ParseDate(IElement element, PrivateMessage pm)
        {
            if (element == null)
            {
                return;
            }

            pm.Date = DateTime.Parse(element.TextContent.Replace("at", string.Empty), CultureInfo.InvariantCulture);
        }
    }
}
