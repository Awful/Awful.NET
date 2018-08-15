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

namespace Awful.Parser.Handlers
{
    public class PostHandler
    {
        public static Post ParsePost(IElement doc)
        {
            var post = new Post();
            post.User = UserHandler.ParseUserFromPost(doc);

            post.HasSeen = doc.QuerySelector(@"[class=""seen1""]") != null || doc.QuerySelector(@"[class=""seen2""]") != null;

            var threadBody = doc.QuerySelector(".postbody");
            if (threadBody != null)
            {
                var attachments = threadBody.QuerySelectorAll(@"[src*=""attachment.php""]");
                foreach(var attachment in attachments)
                {
                    attachment.SetAttribute("src", $"https://forums.somethingawful.com/{attachment.Attributes["src"].Value}");
                }
                post.PostHtml = threadBody.InnerHtml;
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
    }
}
