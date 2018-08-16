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
namespace Awful.Parser.Handlers
{
    public class UserHandler
    {
        public static User ParseUserFromProfilePage(long userId, IHtmlDocument doc)
        {
            var user = new User();
            user.Id = userId;

            var authorTd = doc.QuerySelector(@"[class*=""userinfo""]");
            ParseUserInfoElement(user, authorTd);

            return user;
        }

        public static User ParseUserFromPost(IElement doc)
        {
            var user = new User();

            var authorTd = doc.QuerySelector(@"[class*=""userid""]");

            var userId = authorTd.ClassList.First(n => n.Contains("userid-")).Trim().Replace("userid-", string.Empty);
            user.Id = Convert.ToInt64(userId);

            ParseUserInfoElement(user, authorTd);

            return user;
        }

        private static void ParseUserInfoElement (User user, IElement authorTd)
        {
            var authorTitle = authorTd.QuerySelector(".author");
            user.Username = authorTitle.TextContent;
            user.Roles = authorTitle.ClassName;
            user.Title = authorTitle.GetAttribute("title");
            var userTitleHtml = authorTd.QuerySelector(".title");

            var registered = authorTd.QuerySelector(".registered");

            if (registered != null)
            {
                DateTime reg = new DateTime();
                DateTime.TryParse(registered.TextContent, out reg);
                user.DateJoined = reg;
            }

            if (userTitleHtml == null)
                return;

            user.AvatarHtml = userTitleHtml.InnerHtml;
            user.AvatarTitle = userTitleHtml.TextContent.Trim();

            var userImg = userTitleHtml.QuerySelector(@"img[src*=""avatar""]");
            if (userImg != null)
                user.AvatarLink = userImg.GetAttribute("src");

            if (user.AvatarLink == null)
            {
                var userImgAv = userTitleHtml.QuerySelector(@"img[src*=""titles""]");
                if (userImgAv != null)
                    user.AvatarLink = userImgAv.GetAttribute("src");
            }

            var gangTagImg = userTitleHtml.QuerySelector(@"img[src*=""gangtags""]");
            if (gangTagImg != null)
                user.AvatarGangTagLink = gangTagImg.GetAttribute("src");
        }
    }
}
