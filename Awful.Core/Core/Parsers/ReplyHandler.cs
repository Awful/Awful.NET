using Awful.Models.Posts;
using Awful.Models.Replies;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Awful.Parsers
{
    public class ReplyHandler
    {
        public static void ParsePostPreview(Post post, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode[] replyNodes = doc.DocumentNode.Descendants("div").ToArray();

            HtmlNode previewNode =
                replyNodes.First(node => node.GetAttributeValue("class", "").Equals("inner postbody"));
            post = new Post { PostHtml = previewNode.OuterHtml };
        }

        public static void ParsePostReply(ForumReply forumReplyEntity, long postId, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode[] formNodes = doc.DocumentNode.Descendants("input").ToArray();

            HtmlNode bookmarkNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("bookmark"));

            HtmlNode[] textAreaNodes = doc.DocumentNode.Descendants("textarea").ToArray();

            HtmlNode textNode =
                textAreaNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("message"));

            forumReplyEntity = new ForumReply();
            var quote = WebUtility.HtmlDecode(textNode.InnerText);
            string bookmark = bookmarkNode.OuterHtml.Contains("checked") ? "yes" : "no";
            forumReplyEntity.MapEditPostInformation(quote, postId, bookmark);
        }

        public static void ParsePreviousPosts(List<Post> forumThreadPosts, string html)
        {
            HtmlDocument doc2 = new HtmlDocument();
            doc2.LoadHtml(html);

            HtmlNode threadNode =
               doc2.DocumentNode.Descendants("div")
                   .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));

            foreach (
                HtmlNode postNode in
                    threadNode.Descendants("table")
                        .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post")))
            {
                var post = new Post();
                PostHandler.ParsePost(post, postNode);
                forumThreadPosts.Add(post);
            }
        }

        public static void ParseReplyCookies(ForumReply forumReplyEntity, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode[] formNodes = doc.DocumentNode.Descendants("input").ToArray();

            HtmlNode formKeyNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("formkey"));

            HtmlNode formCookieNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("form_cookie"));

            HtmlNode bookmarkNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("bookmark"));

            HtmlNode[] textAreaNodes = doc.DocumentNode.Descendants("textarea").ToArray();

            HtmlNode textNode =
                textAreaNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("message"));

            HtmlNode threadIdNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("threadid"));

            var forumThreadPosts = new List<Post>();

            HtmlNode threadNode =
               doc.DocumentNode.Descendants("div")
                   .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));

            foreach (
                HtmlNode postNode in
                    threadNode.Descendants("table")
                        .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post")))
            {
                var post = new Post();
                PostHandler.ParsePost(post, postNode);
                forumThreadPosts.Add(post);
            }

            string formKey = formKeyNode.GetAttributeValue("value", "");
            string formCookie = formCookieNode.GetAttributeValue("value", "");
            string quote = WebUtility.HtmlDecode(textNode.InnerText);
            string threadIdTest = threadIdNode.GetAttributeValue("value", "");
            forumReplyEntity.MapThreadInformation(formKey, formCookie, quote, threadIdTest);
            forumReplyEntity.ForumPosts = forumThreadPosts;
        }
    }
}
