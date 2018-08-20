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
using Newtonsoft.Json;

namespace Awful.Parser.Handlers
{
    public class PostHandler
    {
        public static Post ParsePost(IHtmlDocument parent, IElement doc)
        {
            var post = new Post();
            post.User = UserHandler.ParseUserFromPost(doc);
            post.PostId = Convert.ToInt64(doc.Id.Replace("post", ""));
            var authorTd = doc.QuerySelector(@"[class*=""userid""]");
            authorTd.Remove();

            post.HasSeen = doc.QuerySelector(@"[class=""seen1""]") != null || doc.QuerySelector(@"[class=""seen2""]") != null;

            var threadBody = doc.QuerySelector(".postbody");
            if (threadBody != null)
            {
                var jerkBody = threadBody.QuerySelector(@"a[title=""DON'T DO IT!!""]");
                if (jerkBody != null)
                {
                    post.IsIgnored = true;
                }
                else
                {
                    var imgurGifs = threadBody.QuerySelectorAll(@"[src*=""imgur.com""][src*="".gif""]");
                    for (var i = 0; i < imgurGifs.Length; i++)
                    {
                        var imgurGif = imgurGifs[i];
                        var div = parent.CreateElement("div");
                        div.ClassList.Add("gifWrap");
                        var newImgur = parent.CreateElement("img");
                        newImgur.ClassList.Add("imgurGif");
                        newImgur.SetAttribute("data-originalurl", imgurGif.GetAttribute("src"));
                        newImgur.SetAttribute("data-posterurl", imgurGif.GetAttribute("src").Replace(".gif", "h.jpg"));
                        newImgur.SetAttribute("src", imgurGif.GetAttribute("src").Replace(".gif", "h.jpg"));
                        div.AppendChild(newImgur);
                        imgurGif.Replace(div);
                    }
                    var attachments = threadBody.QuerySelectorAll(@"[src*=""attachment.php""]");
                    foreach (var attachment in attachments)
                    {
                        attachment.SetAttribute("src", $"https://forums.somethingawful.com/{attachment.Attributes["src"].Value}");
                    }
                }
                post.PostHtml = HtmlEncode(threadBody.InnerHtml);
            }

            return post;
        }

        public static Post ParsePostPreview(IHtmlDocument doc)
        {
            var post = new Post();

            var threadBody = doc.QuerySelector(".postbody");
            if (threadBody != null)
                post.PostHtml = threadBody.OuterHtml;

            return post;
        }

        private static string HtmlEncode(string text)
        {
            // In order to get Unicode characters fully working, we need to first encode the entire post.
            // THEN we decode the bits we can safely pass in, like single/double quotes.
            // If we don't, the post format will be screwed up.
            char[] chars = WebUtility.HtmlEncode(text).ToCharArray();
            var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            result.Replace("&quot;", "\"");
            result.Replace("&#39;", @"'");
            result.Replace("&lt;", @"<");
            result.Replace("&gt;", @">");
            return result.ToString();
        }
    }
}
