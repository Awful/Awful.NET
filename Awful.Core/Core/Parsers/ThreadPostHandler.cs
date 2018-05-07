using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Awful.Parsers
{
    public class ThreadPostHandler
    {
        public static void GetThread(Thread thread, string html, string url, string responseUrl)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            CheckPaywall(doc);
            GetThreadInfo(thread, doc, url, responseUrl);
            GetThreadPosts(thread.Posts, doc);
        }

        public static async Task GetThread(ThreadPosts threadPosts, string html, string url, string responseUrl, bool autoplayGifs = true)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            CheckPaywall(doc);
            GetThreadInfo(threadPosts.ForumThread, doc, url, responseUrl);
            await EmbedTweets(doc);
            if (!autoplayGifs)
                RemoveAutoplayGifs(doc);
            GetThreadPosts(threadPosts.Posts, doc);
        }

        public static async Task EmbedTweets (HtmlDocument doc)
        {
            var tweets = doc.DocumentNode.Descendants("a").Where(n => n.InnerHtml.Contains("twitter.com"));
            using (var client = new HttpClient())
            {
                foreach (var tweet in tweets.ToList())
                {
                    try
                    {
                        var result = await client.GetAsync($"https://publish.twitter.com/oembed?omit_script=true&url={tweet.InnerHtml}");
                        dynamic tweetResult = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                        var node = doc.CreateElement("div");
                        node.InnerHtml = tweetResult.html;
                        tweet.ParentNode.ReplaceChild(node, tweet);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
        }

        public static void RemoveAutoplayGifs(HtmlDocument doc)
        {
            var imgs = doc.DocumentNode.Descendants("img").Where(n => n.GetAttributeValue("src", "").Contains(".gif") && !n.GetAttributeValue("src", "").Contains("somethingawful"));
            foreach(var img in imgs.ToList())
            {
                var imgUrlString = img.GetAttributeValue("src", "");
                if (string.IsNullOrEmpty(imgUrlString))
                    continue;
                var url = new Uri(imgUrlString);

                var hostName = url.Host.ToLowerInvariant();
                if (hostName.Contains("imgur.com"))
                {
                    imgUrlString = imgUrlString.Replace(".gif", "h.jpg");
                }
                else
                {
                    switch (hostName)
                    {
                        case "i.kinja-img.com":
                            imgUrlString = imgUrlString.Replace(".gif", ".jpg");
                            break;
                        case "i.giphy.com":
                            imgUrlString = imgUrlString.Replace("://i.giphy.com", "s://media.giphy.com/media");
                            imgUrlString = imgUrlString.Replace(".gif", "/200_s.gif");
                            break;
                        case "giant.gfycat.com":
                            imgUrlString = imgUrlString.Replace("giant.gfycat.com", "thumbs.gfycat.com");
                            imgUrlString = imgUrlString.Replace(".gif", "-poster.jpg");
                            break;
                        default:
                            break;
                    }
                }

                var wrapper = doc.CreateElement("div");
                var newImg = doc.CreateElement("img");
                foreach(var classnames in img.GetClasses())
                {
                    newImg.AddClass(classnames);
                }
                wrapper.AddClass("imgurGif");
                newImg.SetAttributeValue("data-originalurl", img.GetAttributeValue("src", "").ToLowerInvariant());
                newImg.SetAttributeValue("data-posterurl", imgUrlString);
                newImg.SetAttributeValue("src", imgUrlString);
                wrapper.AppendChild(newImg);
                img.ParentNode.ReplaceChild(wrapper, img);
            }
        }

        public static void GetThreadPosts(Thread thread, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            CheckPaywall(doc);
            GetThreadPosts(thread.Posts, doc);
        }

        public static void CheckPaywall(HtmlDocument doc)
        {
            if (
                   doc.DocumentNode.InnerText.Contains(
                       "Sorry, you must be a registered forums member to view this page."))
            {
                throw new Exception("paywall");
            }
        }
        public static void GetThreadPosts(List<Post> forumThreadPosts, HtmlDocument doc)
        {
            HtmlNode threadNode =
                   doc.DocumentNode.Descendants("div")
                       .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));

            foreach (
               HtmlNode postNode in
                   threadNode.Descendants("table")
                       .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post") && node.GetAttributeValue("data-idx", string.Empty) != ""))
            {
                var post = new Post();
                PostHandler.ParsePost(post, postNode);
                if (post.PostId == 0) continue;
                var postBodyNode =
                    postNode.Descendants("td")
                        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("postbody"));
                var query =
                    postBodyNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("class", string.Empty) == "bbc-block");
                foreach (var item in query.ToList())
                {
                    var newHeadNode = HtmlNode.CreateNode(item.OuterHtml);
                    item.ParentNode.ReplaceChild(newHeadNode.ParentNode, item);
                }

                var h4Query = postBodyNode.Descendants("h4");
                foreach (var h4 in h4Query.ToList())
                {
                    var newHeadNode = HtmlNode.CreateNode($"<h4>{h4.InnerText}</h4>");
                    h4.ParentNode.ReplaceChild(newHeadNode.ParentNode, h4);
                }

                forumThreadPosts.Add(post);
            }
        }
        public static void GetThreadInfo(Thread forumThread, HtmlDocument doc, string url, string responseUri)
        {
            try
            {
                ParseFromThread(forumThread, doc);
            }
            catch (Exception exception)
            {
                return;
            }

            try
            {
                var usernameNode = doc.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Equals("loggedinusername"));
                forumThread.LoggedInUserName = usernameNode != null ? usernameNode.InnerText : string.Empty;
                forumThread.IsLoggedIn = forumThread.LoggedInUserName != "Unregistered Faggot";
                string[] test = responseUri.Split('#');
                if (test.Length > 1 && test[1].Contains("pti"))
                {
                    forumThread.ScrollToPost = Int32.Parse(Regex.Match(responseUri.Split('#')[1], @"\d+").Value) - 1;
                    forumThread.ScrollToPostString = string.Concat("#", responseUri.Split('#')[1]);
                }

                var query = HtmlHelpers.ParseQueryString(new Uri(url).Query);

                if (query.ContainsKey("pagenumber"))
                {
                    forumThread.CurrentPage = Convert.ToInt32(query["pagenumber"]);
                }
            }
            catch (Exception exception)
            {

            }
        }

        private static void ParseFromThread(Thread threadEntity, HtmlDocument threadDocument)
        {
            var title = threadDocument.DocumentNode.Descendants("title").FirstOrDefault();

            if (title != null)
            {
                threadEntity.Name = WebUtility.HtmlDecode(title.InnerText.Replace(" - The Something Awful Forums", string.Empty));
            }

            var threadIdNode = threadDocument.DocumentNode.Descendants("body").First();
            threadEntity.ThreadId = Convert.ToInt32(threadIdNode.GetAttributeValue("data-thread", string.Empty));

            var usernameNode = threadDocument.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Equals("loggedinusername"));
            threadEntity.LoggedInUserName = usernameNode != null ? usernameNode.InnerText : string.Empty;

            threadEntity.Location = string.Format(EndPoints.ThreadPage, threadEntity.ThreadId);
            var pageNavigationNode = threadDocument.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("pages top"));
            if (string.IsNullOrWhiteSpace(pageNavigationNode.InnerHtml))
            {
                threadEntity.TotalPages = 1;
                threadEntity.CurrentPage = 1;
            }
            else
            {
                try
                {
                    var lastDisabledPage = pageNavigationNode.Descendants("span").LastOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("disabled"));
                    if (lastDisabledPage != null)
                    {
                        Regex re = new Regex(@"\d+");
                        Match m = re.Match(lastDisabledPage.InnerText);

                        if (m.Success)
                        {
                            threadEntity.TotalPages = Convert.ToInt32(m.Value);
                        }
                    }
                }
                catch (Exception)
                {
                    // Ignore
                }

                var lastPageNode = pageNavigationNode.Descendants("a").FirstOrDefault(node => node.GetAttributeValue("title", string.Empty).Equals("Last page"));
                if (lastPageNode != null)
                {
                    string urlHref = lastPageNode.GetAttributeValue("href", string.Empty);
                    var query = HtmlHelpers.ParseQueryString(new Uri(EndPoints.BaseUrl + urlHref).Query);
                    if (query.ContainsKey("pagenumber"))
                        threadEntity.TotalPages = Convert.ToInt32(query["pagenumber"]);
                }

                var pageSelector = pageNavigationNode.Descendants("select").FirstOrDefault();

                var selectedPage = pageSelector.Descendants("option")
                    .FirstOrDefault(node => node.GetAttributeValue("selected", string.Empty).Equals("selected"));

                threadEntity.CurrentPage = Convert.ToInt32(selectedPage.GetAttributeValue("value", string.Empty));
            }
        }
    }
}
