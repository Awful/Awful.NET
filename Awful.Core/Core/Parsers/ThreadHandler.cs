using Awful.Models.Forums;
using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Models.Web;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parsers
{
    public class ThreadHandler
    {
        public static void ParseForumThreads(List<Thread> forumThreadList, string html, int forumId)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var forumNode =
                doc.DocumentNode.Descendants()
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("threadlist"));

            foreach (
       HtmlNode threadNode in
           forumNode.Descendants("tr")
               .Where(node => node.GetAttributeValue("class", string.Empty).StartsWith("thread")))
            {
                var threadEntity = new Thread { ForumId = forumId };
                ParseThreadHtml(threadEntity, threadNode);
                forumThreadList.Add(threadEntity);
            }
        }
        public static void ParseNewThreadPreview(Post post, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode[] replyNodes = doc.DocumentNode.Descendants("div").ToArray();

            HtmlNode previewNode =
                replyNodes.FirstOrDefault(node => node.GetAttributeValue("class", "").Equals("inner postbody"));
            post.PostHtml = previewNode.OuterHtml;
        }
        public static void ParseNewThread(NewThread newForumEntity, string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode[] formNodes = doc.DocumentNode.Descendants("input").ToArray();

            HtmlNode formKeyNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("formkey"));

            HtmlNode formCookieNode =
                formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("form_cookie"));

            string formKey = formKeyNode.GetAttributeValue("value", "");
            string formCookie = formCookieNode.GetAttributeValue("value", "");
            newForumEntity.FormKey = formKey;
            newForumEntity.FormCookie = formCookie;
        }
        public static void ParseThreadList(List<Thread> forumThreadList, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode forumNode =
                doc.DocumentNode.Descendants()
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("threadlist"));


            foreach (
                HtmlNode threadNode in
                    forumNode.Descendants("tr")
                        .Where(node => node.GetAttributeValue("class", string.Empty).StartsWith("thread")))
            {
                var threadEntity = new Thread { ForumId = 0, IsBookmark = true };
                ThreadHandler.ParseThreadHtml(threadEntity, threadNode);
                forumThreadList.Add(threadEntity);
            }
        }
        private static void ParseHasSeenThread(Thread threadEntity, HtmlNode threadNode)
        {
            threadEntity.HasSeen = threadNode.GetAttributeValue("class", string.Empty).Contains("seen");
        }

        private static void ParseThreadTitleAnnouncement(Thread threadEntity, HtmlNode threadNode)
        {
            var titleNode = threadNode.Descendants("a")
               .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("thread_title")) ??
                           threadNode.Descendants("a")
                   .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("announcement"));

            threadEntity.IsAnnouncement = titleNode != null &&
                titleNode.GetAttributeValue("class", string.Empty).Equals("announcement");

            threadEntity.Name =
                titleNode != null ? WebUtility.HtmlDecode(titleNode.InnerText) : "BLANK TITLE?!?";
        }

        private static void ParseThreadKilledBy(Thread threadEntity, HtmlNode threadNode)
        {
            threadEntity.KilledBy =
                threadNode.Descendants("a")
                    .LastOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author")) != null ? threadNode.Descendants("a")
                    .LastOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author")).InnerText : string.Empty;
        }

        private static void ParseThreadIsSticky(Thread threadEntity, HtmlNode threadNode)
        {
            threadEntity.IsSticky =
                threadNode.Descendants("td")
                    .Any(node => node.GetAttributeValue("class", string.Empty).Contains("title_sticky"));
        }

        private static void ParseThreadIsLocked(Thread threadEntity, HtmlNode threadNode)
        {
            threadEntity.IsLocked = threadNode.GetAttributeValue("class", string.Empty).Contains("closed");
        }

        private static void ParseThreadCanMarkAsUnread(Thread threadEntity, HtmlNode threadNode)
        {
            threadEntity.CanMarkAsUnread =
                threadNode.Descendants("a").Any(node => node.GetAttributeValue("class", string.Empty).Equals("x"));
        }

        private static void ParseThreadAuthor(Thread threadEntity, HtmlNode threadNode)
        {
            threadEntity.Author =
                WebUtility.HtmlDecode(threadNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author"))
                    .InnerText);
        }

        private static void ParseThreadRepliesSinceLastOpened(Thread threadEntity, HtmlNode threadNode)
        {
            if (threadNode.Descendants("a").Any(node => node.GetAttributeValue("class", string.Empty).Equals("count")))
            {
                threadEntity.RepliesSinceLastOpened =
                    Convert.ToInt32(
                        threadNode.Descendants("a")
                            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("count"))
                            .InnerText);
            }
        }

        private static void ParseThreadReplyCount(Thread threadEntity, HtmlNode threadNode)
        {
            try
            {
                threadEntity.ReplyCount =
                threadNode.Descendants("td")
                    .Any(node => node.GetAttributeValue("class", string.Empty).Contains("replies"))
                    ? Convert.ToInt32(
                        threadNode.Descendants("td")
                            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("replies"))
                            .InnerText)
                    : 1;
            }
            catch (Exception)
            {
                threadEntity.ReplyCount = 0;
            }
        }

        private static void ParseThreadViewCount(Thread threadEntity, HtmlNode threadNode)
        {
            try
            {
                threadEntity.ViewCount =
               threadNode.Descendants("td")
                   .Any(node => node.GetAttributeValue("class", string.Empty).Contains("views"))
                   ? Convert.ToInt32(
                       threadNode.Descendants("td")
                           .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("views"))
                           .InnerText)
                   : 1;
            }
            catch (Exception)
            {
                threadEntity.ViewCount = 0;
            }
        }

        private static void ParseThreadRating(Thread threadEntity, HtmlNode threadNode)
        {
            var threadRatingUrl = threadNode.Descendants("td")
                .Any(node => node.GetAttributeValue("class", string.Empty).Contains("rating")) &&
                                  threadNode.Descendants("td")
                                      .FirstOrDefault(
                                          node => node.GetAttributeValue("class", string.Empty).Equals("rating"))
                                      .Descendants("img")
                                      .Any()
                ? threadNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("rating"))
                    .Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", string.Empty) : null;

            if (!string.IsNullOrEmpty(threadRatingUrl))
            {
                threadEntity.RatingImageUrl = threadRatingUrl;
                threadEntity.RatingImage = Path.GetFileNameWithoutExtension(threadRatingUrl);
            }
        }

        private static void ParseThreadTotalPages(Thread threadEntity)
        {
            threadEntity.TotalPages = (threadEntity.ReplyCount / 40) + 1;
        }

        private static void ParseThreadId(Thread threadEntity, HtmlNode threadNode)
        {
            var titleNode = threadNode.Descendants("a")
              .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("thread_title")) ??
                          threadNode.Descendants("a")
                  .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("announcement"));

            if (titleNode == null) return;

            threadEntity.Location = EndPoints.BaseUrl +
                                    titleNode.GetAttributeValue("href", string.Empty) + EndPoints.PerPage;

            threadEntity.ThreadId =
                Convert.ToInt32(
                    titleNode
                        .GetAttributeValue("href", string.Empty)
                        .Split('=')[1]);
        }

        private static void ParseThreadIcon(Thread threadEntity, HtmlNode threadNode)
        {
            HtmlNode first =
               threadNode.Descendants("td")
                   .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon"));
            if (first != null)
            {
                var testImageString = first.Descendants("img").FirstOrDefault().GetAttributeValue("src", string.Empty); ;
                if (!string.IsNullOrEmpty(testImageString))
                {
                    threadEntity.ImageIconUrl = testImageString.Replace("http:", "https:");
                    threadEntity.ImageIconLocation = Path.GetFileNameWithoutExtension(testImageString);
                }
            }
        }

        private static void ParseStoreThreadIcon(Thread threadEntity, HtmlNode threadNode)
        {
            HtmlNode second =
    threadNode.Descendants("td")
        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon2"));
            if (second == null) return;
            try
            {
                var testImageString = second.Descendants("img").FirstOrDefault().GetAttributeValue("src", string.Empty);
                if (!string.IsNullOrEmpty(testImageString))
                {
                    threadEntity.StoreImageIconUrl = testImageString;
                    threadEntity.StoreImageIconLocation = Path.GetFileNameWithoutExtension(testImageString);
                }
            }
            catch (Exception)
            {
                threadEntity.StoreImageIconLocation = null;
            }
        }

        public static void ParseThreadHtml(Thread threadEntity, HtmlNode threadNode)
        {
            try
            {
                ParseHasSeenThread(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Has Seen' element {exception}");
            }

            try
            {
                ParseThreadTitleAnnouncement(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Thread/Announcement' element {exception}");
            }

            try
            {
                ParseThreadKilledBy(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Killed By' element {exception}");
            }

            try
            {
                ParseThreadIsSticky(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Is Thread Sticky' element {exception}");
            }

            try
            {
                ParseThreadIsLocked(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Thread Locked' element {exception}");
            }

            try
            {
                ParseThreadCanMarkAsUnread(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Failed to parse 'Can mark as thread as unread' element {exception}");
            }

            try
            {
                threadEntity.HasBeenViewed = threadEntity.CanMarkAsUnread;
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Has Been Viewed' element {exception}");
            }

            try
            {
                ParseThreadAuthor(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Thread Author' element {exception}");
            }

            try
            {
                ParseThreadRepliesSinceLastOpened(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Failed to parse 'Replies since last opened' element {exception}");
            }

            try
            {
                ParseThreadReplyCount(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Reply count' element {exception}");
            }

            try
            {
                ParseThreadViewCount(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'View Count' element {exception}");
            }

            try
            {
                ParseThreadRating(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Thread Rating' element {exception}");
            }

            try
            {
                ParseThreadTotalPages(threadEntity);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Total Pages' element {exception}");
            }

            try
            {
                ParseThreadId(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Thread Id' element {exception}");
            }

            try
            {
                ParseThreadIcon(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Thread Icon' element {exception}");
            }

            try
            {
                ParseStoreThreadIcon(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to parse 'Store thread icon' element {exception}");
            }

        }
    }
}
