using Awful.Models.Messages;
using Awful.Models.Posts;
using Awful.Tools;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Awful.Parsers
{
    public class PrivateMessageHandler
    {
        public static void Parse(List<Post> postList, string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode[] replyNodes = doc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("id", "").Equals("thread")).ToArray();

            HtmlNode threadNode = replyNodes.FirstOrDefault(node => node.GetAttributeValue("id", "").Equals("thread"));

            IEnumerable<HtmlNode> postNodes =
                threadNode.Descendants("table")
                    .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post"));
            foreach (
     HtmlNode postNode in
         postNodes)
            {
                var post = new Post();
                PostHandler.ParsePost(post, postNode);
                postList.Add(post);
            }
        }

        public static void Parse(List<PrivateMessage> privateMessageEntities, string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode forumNode =
                doc.DocumentNode.Descendants("tbody").FirstOrDefault();


            foreach (
                HtmlNode threadNode in
                    forumNode.Descendants("tr"))
            {
                var threadEntity = new PrivateMessage();
                try
                {
                    Parsers.PrivateMessageHandler.ParsePrivateMessage(threadEntity, threadNode);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to parse private message list", ex);
                }
                privateMessageEntities.Add(threadEntity);
            }
        }
        public static void ParsePrivateMessage(PrivateMessage pmEntity, HtmlNode rowNode)
        {
            pmEntity.Status =
                rowNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("status"))
                    .Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", string.Empty);

            var icon = rowNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon"))
                    .Descendants("img")
                    .FirstOrDefault();

            if (icon != null)
            {
                pmEntity.Icon = new Models.PostIcons.PostIcon() { ImageUrl = icon.GetAttributeValue("src", string.Empty) };
                pmEntity.ImageIconLocation = Path.GetFileNameWithoutExtension(icon.GetAttributeValue("src", string.Empty));
            }

            var titleNode = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("title"));

            pmEntity.Title =
               titleNode
                    .InnerText.Replace("\n", string.Empty).Trim();

            string titleHref = titleNode.Descendants("a").FirstOrDefault().GetAttributeValue("href", string.Empty).Replace("&amp;", "&");

            pmEntity.MessageUrl = EndPoints.BaseUrl + titleHref;

            pmEntity.Sender = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("sender"))
                .InnerText;
            pmEntity.Date = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("date"))
                .InnerText;
        }
    }
}
