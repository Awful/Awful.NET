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
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Users;

namespace Awful.Parser.Handlers
{
    public class ThreadHandler
    {
        public static void CheckPaywall(IHtmlDocument doc)
        {
            var test = doc.QuerySelector(".inner");
            if (test != null)
            {
                if (test.TextContent.Contains("Sorry, you must be a registered forums member to view this page."))
                {
                    throw new Exception("paywall");
                }
            }
        }

        public static List<Thread> ParseForumThreadList(IHtmlDocument doc, int forumId)
        {
            CheckPaywall(doc);
            var forumThreadList = new List<Thread>();
            var threadTableList = doc.QuerySelector("#forum");
            if (threadTableList == null)
                throw new FormatException("Could not find thread list in given HTML");

            var rows = threadTableList.QuerySelectorAll("tr");

            foreach(var row in rows)
            {
                if (row.Id == null)
                    continue;
                var thread = new Thread
                {
                    ThreadId = Convert.ToInt32(row.Id.Replace("thread", ""))
                };
                ParseStar(row.QuerySelector(".star"), thread);
                ParseIcon(row.QuerySelector(".icon"), thread);
                ParseTitle(row.QuerySelector(".title"), thread);
                ParseAuthor(row.QuerySelector(".author"), thread);
                ParseReplies(row.QuerySelector(".replies"), thread);
                ParseViews(row.QuerySelector(".views"), thread);
                ParseRating(row.QuerySelector(".rating"), thread);
                ParseLastPost(row.QuerySelector(".lastpost"), thread);
                ParseLastSeen(row.QuerySelector(".lastseen"), thread);
                forumThreadList.Add(thread);
            }

            return forumThreadList;
        }

        public static NewThread ParseNewThread(IHtmlDocument document)
        {
            return new NewThread
            {
                FormKey = document.QuerySelector(@"input[name=""formkey""]").GetAttribute("value"),
                FormCookie = document.QuerySelector(@"input[name=""form_cookie""]").GetAttribute("value")
            };
        }

        public static Thread ParseThread(IHtmlDocument doc, Thread thread, string responseUri = "")
        {
            CheckPaywall(doc);
            ParseArchive(doc, thread);
            ParseThreadInfo(doc, thread, responseUri);
            ParseThreadPageNumbers(doc, thread);
            ParseThreadPage(doc, thread);
            ParseThreadPosts(doc, thread);
            return thread;
        }

        public static List<Post> ParsePreviousPosts(IHtmlDocument doc)
        {
            var posts = new List<Post>();
            var threadDivTableHolder = doc.QuerySelector("#thread");
            foreach (var threadTable in threadDivTableHolder.QuerySelectorAll("table"))
            {
                if (string.IsNullOrEmpty(threadTable.GetAttribute("data-idx")))
                    continue;
                posts.Add(PostHandler.ParsePost(threadTable));
            }
            return posts;
        }

        private static void ParseThreadPosts(IHtmlDocument doc, Thread thread)
        {
            var threadDivTableHolder = doc.QuerySelector("#thread");
            foreach(var threadTable in threadDivTableHolder.QuerySelectorAll("table"))
            {
                if (string.IsNullOrEmpty(threadTable.GetAttribute("data-idx")))
                    continue;
                thread.Posts.Add(PostHandler.ParsePost(threadTable));
            }
        }

        private static void ParseThreadPage(IHtmlDocument doc, Thread thread)
        {
            thread.Name = doc.Title.Replace(" - The Something Awful Forums", string.Empty);
            var threadBody = doc.QuerySelector("body");
            thread.ThreadId = Convert.ToInt32(threadBody.GetAttribute("data-thread"));
            thread.ForumId = Convert.ToInt32(threadBody.GetAttribute("data-forum"));
        }

        private static void ParseThreadInfo(IHtmlDocument doc, Thread thread, string responseUri = "")
        {
            thread.LoggedInUserName = doc.QuerySelector("#loggedinusername").TextContent;
            thread.IsLoggedIn = thread.LoggedInUserName != "Unregistered Faggot";
            if (string.IsNullOrEmpty(responseUri))
                return;
            string[] test = responseUri.Split('#');

            if (test.Length > 1 && test[1].Contains("pti"))
            {
                thread.ScrollToPost = Int32.Parse(Regex.Match(responseUri.Split('#')[1], @"\d+").Value) - 1;
                thread.ScrollToPostString = string.Concat("#", responseUri.Split('#')[1]);
            }
        }

        private static void ParseArchive(IHtmlDocument doc, Thread thread)
        {
            var archiveButton = doc.QuerySelector(@"img[src*=""button-archive""]");
            if (archiveButton != null)
                thread.IsArchived = true;
        }

        private static void ParseStar(IElement element, Thread thread)
        {
            if (element == null)
                return;
            var starColor = element.ClassList.FirstOrDefault(n => n.Contains("bm") && !n.Contains("bm-1"));
            if (string.IsNullOrEmpty(starColor))
                return;
            thread.StarColor = starColor;
            thread.IsBookmark = true;
        }

        private static void ParseLastSeen(IElement element, Thread thread)
        {
            if (element == null)
                return;
            thread.HasSeen = true;
            var count = element.QuerySelector(".count");
            if (count == null)
                return;
            thread.RepliesSinceLastOpened = Convert.ToInt32(count.TextContent);
        }

        private static void ParseIcon(IElement element, Thread thread)
        {
            if (element == null)
                return;
            var img = element.QuerySelector("img");
            thread.ImageIconUrl = img.GetAttribute("src");
            thread.ImageIconLocation = Path.GetFileNameWithoutExtension(thread.ImageIconUrl);
        }

        private static void ParseTitle(IElement element, Thread thread)
        {
            if (element == null)
                return;
            if (element.ClassList.Contains("title_sticky"))
                thread.IsSticky = true;
            var threadList = element.QuerySelector(".thread_title");
            thread.Name = threadList.TextContent;
        }

        private static void ParseAuthor(IElement element, Thread thread)
        {
            if (element == null)
                return;
            var authorLink = element.QuerySelector("a");
            var user = new User();
            user.Id = Convert.ToInt64(authorLink.GetAttribute("href").Split('=').Last());
            user.Username = authorLink.TextContent;
            thread.Author = user.Username;
            thread.AuthorId = user.Id;
        }

        private static void ParseReplies(IElement element, Thread thread)
        {
            if (element == null)
                return;
            thread.ReplyCount = Convert.ToInt32(element.TextContent);
            thread.TotalPages = (thread.ReplyCount / 40) + 1;
        }

        private static void ParseViews(IElement element, Thread thread)
        {
            if (element == null)
                return;
            thread.ViewCount = Convert.ToInt32(element.TextContent);
        }

        private static void ParseRating(IElement element, Thread thread)
        {
            if (element == null || element.ChildElementCount <= 0)
                return;
            var img = element.QuerySelector("img");
            thread.RatingImageUrl = img.GetAttribute("src");
            thread.RatingImage = Path.GetFileNameWithoutExtension(thread.RatingImageUrl);
            var firstSplit = img.GetAttribute("title").Split('-');
            thread.TotalRatingVotes = Convert.ToInt32(Regex.Match(firstSplit[0], @"\d+").Value);
            thread.Rating = Convert.ToDecimal(Regex.Match(firstSplit[1], @"[\d]{1,4}([.,][\d]{1,2})?").Value);
        }

        private static void ParseLastPost(IElement element, Thread thread)
        {
            if (element == null)
                return;
            var date = element.QuerySelector(".date");
            var author = element.QuerySelector(".author");
            var user = new User();
            thread.KilledOn = DateTime.Parse(date.TextContent);
            user.Id = Convert.ToInt64(author.GetAttribute("href").Split('=').Last());
            user.Username = author.TextContent;
            thread.KilledById = user.Id;
            thread.KilledBy = user.Username;
        }

        private static void ParseThreadPageNumbers(IHtmlDocument doc, Thread thread)
        {
            try
            {
                var pages = doc.QuerySelector(".pages");
                if (pages == null)
                {
                    thread.CurrentPage = 1;
                    thread.TotalPages = 1;
                    return;
                }
                var select = pages.QuerySelector("select");
                if (select == null)
                {
                    thread.CurrentPage = 1;
                    thread.TotalPages = 1;
                    return;
                }
                var selectedPageItem = select.QuerySelector("option:checked");
                thread.CurrentPage = Convert.ToInt32(selectedPageItem.TextContent);
                thread.TotalPages = select.ChildElementCount;
            }
            catch (Exception)
            {
                thread.CurrentPage = 1;
                thread.TotalPages = 1;
                return;
            }
        }
    }
}
