// <copyright file="PrivateMessageHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;
using Awful.Core.Entities.Messages;
using Awful.Core.Exceptions;
using System.Globalization;

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
                throw new AwfulParserException($"{nameof(PrivateMessage)}: ParseList: privateMessageList");
            }

            var pmListBody = doc.QuerySelector("tbody");
            if (pmListBody == null)
            {
                throw new Exceptions.AwfulParserException("Failed to find pmListBody while parsing Private Message Page.");
            }

            var pmListRows = pmListBody.QuerySelectorAll("tr");
            foreach (var pmRow in pmListRows)
            {
                var statusItem = pmRow.QuerySelector(".status");
                var img = statusItem?.QuerySelector("img");
                if (img is null)
                {
                    throw new AwfulParserException($"{nameof(PrivateMessage)}: ParseList: img");
                }

                var statusImageIconEndpoint = img.TryGetAttribute("src");
                var statusImageIconLocation = Path.GetFileNameWithoutExtension(statusImageIconEndpoint);

                var iconItem = pmRow.QuerySelector(".icon");
                img = iconItem?.QuerySelector("img");

                string? imageIconEndpoint = null;
                if (img is not null)
                {
                    imageIconEndpoint = img.TryGetAttribute("src");
                }

                var titleItem = pmRow.QuerySelector(".title");
                var threadList = titleItem?.QuerySelector("a");
                if (threadList == null)
                {
                    throw new AwfulParserException($"{nameof(PrivateMessage)}: ParseList: threadList");
                }

                var messageEndpoint = threadList.TryGetAttribute("href");
                var privateMessageId = Convert.ToInt32(messageEndpoint.Split('=').Last(), CultureInfo.InvariantCulture);
                var title = threadList.TextContent;

                var senderItem = pmRow.QuerySelector(".sender");
                if (senderItem is null)
                {
                    throw new AwfulParserException($"{nameof(PrivateMessage)}: ParseList: sender");
                }

                var sender = senderItem.TextContent;

                var dateItem = pmRow.QuerySelector(".date");
                if (dateItem is null)
                {
                    throw new AwfulParserException($"{nameof(PrivateMessage)}: ParseList: dateItem");
                }

                var date = DateTime.Parse(dateItem.TextContent.Replace("at", string.Empty), CultureInfo.InvariantCulture);
                privateMessageList.Add(new PrivateMessage(privateMessageId, statusImageIconEndpoint, title, sender, date, messageEndpoint, imageIconEndpoint));
            }

            return privateMessageList;
        }
    }
}
