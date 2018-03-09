using Awful.Models.Users;
using Awful.Tools;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Awful.Parsers
{
    public class UserHandler
    {
        public static void ParseFromUserProfile(User user, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode profileNode = doc.DocumentNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("info"));

            HtmlNode threadNode = doc.DocumentNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));
            ParseFromUserProfile(user, profileNode, threadNode);
        }

        public static void ParseFromUserProfile(User user, HtmlNode profileNode, HtmlNode threadNode)
        {
            HtmlNode additionalNode =
                profileNode.Descendants("dl")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("additional"));
            Dictionary<string, string> additionalProfileAttributes = ParseAdditionalProfileAttributes(additionalNode);

            HtmlNode contactsNode =
                profileNode.Descendants("dl")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("contacts"));
            Dictionary<string, string> contactsProfileAttributes = ParseAdditionalProfileAttributes(contactsNode);

            user = new User
            {
                Username =
                    threadNode.Descendants("dt")
                        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author"))
                        .InnerText,
                AboutUser = string.Empty,
                DateJoined = DateTime.Parse(additionalProfileAttributes["Member Since"]),
                PostCount = int.Parse(additionalProfileAttributes["Post Count"]),
                PostRate = additionalProfileAttributes["Post Rate"]
            };

            var avatarNode = threadNode.Descendants("img").FirstOrDefault();
            if (avatarNode != null)
            {
                user.AvatarLink = avatarNode.GetAttributeValue("src", string.Empty);
            }

            var userPicNode = profileNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("userpic"));
            if (userPicNode != null)
            {
                var userPicImageNode = userPicNode.Descendants("img").FirstOrDefault();
                user.UserPicLink = userPicImageNode != null ? userPicImageNode.GetAttributeValue("src", string.Empty) : "";
            }

            foreach (HtmlNode aboutParagraph in profileNode.Descendants("p"))
            {
                user.AboutUser += WebUtility.HtmlDecode(aboutParagraph.InnerText.WithoutNewLines().Trim()) +
                                  Environment.NewLine + Environment.NewLine;
            }
            if (contactsProfileAttributes.ContainsKey("ICQ"))
            {
                user.IcqContactString = contactsProfileAttributes["ICQ"];
            }
            if (contactsProfileAttributes.ContainsKey("AIM"))
            {
                user.AimContactString = contactsProfileAttributes["AIM"];
            }
            if (contactsProfileAttributes.ContainsKey("Yahoo!"))
            {
                user.YahooContactString = contactsProfileAttributes["Yahoo!"];
            }
            if (contactsProfileAttributes.ContainsKey("Home Page"))
            {
                user.HomePageString = contactsProfileAttributes["Home Page"];
            }
            if (additionalProfileAttributes.ContainsKey("Seller Rating"))
            {
                user.SellerRating = additionalProfileAttributes["Seller Rating"];
            }
            if (additionalProfileAttributes.ContainsKey("Last Post"))
            {
                user.LastPostDate = additionalProfileAttributes["Last Post"];
            }
            if (additionalProfileAttributes.ContainsKey("Location"))
            {
                user.Location = additionalProfileAttributes["Location"];
            }
        }

        private static Dictionary<string, string> ParseAdditionalProfileAttributes(HtmlNode additionalNode)
        {
            IEnumerable<HtmlNode> dts = additionalNode.Descendants("dt");
            IEnumerable<HtmlNode> dds = additionalNode.Descendants("dd");
            Dictionary<string, string> result =
                dts.Zip(dds, (first, second) => new Tuple<string, string>(first.InnerText, second.InnerText))
                    .ToDictionary(k => k.Item1, v => v.Item2);
            // Clean up malformed HTML that results in the "last post" value being all screwy
            return result;
            //string lastPostValue = result["Last Post"];
            //int removalStartIndex = lastPostValue.IndexOf('\n');
            //if (removalStartIndex > 0)
            //{
            //    int lengthToRemove = lastPostValue.Length - removalStartIndex;
            //    result["Last Post"] = lastPostValue.Remove(removalStartIndex, lengthToRemove);
            //}
            //return result;
        }

        public static void ParseNewUserFromPost(User user, HtmlNode postNode)
        {
            user.Username =
                    WebUtility.HtmlDecode(
                        postNode.Descendants("dt")
                            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("author"))
                            .InnerHtml);

            try
            {
                user.Roles = postNode.Descendants("dt")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("author"))
                .GetAttributeValue("class", string.Empty);
            }
            catch (Exception)
            {

            }

            user.IsMod = postNode.Descendants("dt")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("role-mod")) != null;

            user.IsAdmin = postNode.Descendants("dt")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("role-admin")) != null;

            var dateTimeNode = postNode.Descendants("dd")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("registered"));
            if (dateTimeNode != null)
            {
                try
                {
                    user.DateJoined = string.IsNullOrEmpty(dateTimeNode.InnerHtml) ? DateTime.UtcNow : DateTime.Parse(dateTimeNode.InnerHtml);
                }
                catch (Exception)
                {
                    // Parsing failed, so say they joined today.
                    // I blame SA for any parsing failures.
                    user.DateJoined = DateTime.UtcNow;
                }

            }
            HtmlNode avatarTitle =
                postNode.Descendants("dd")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("title"));
            HtmlNode avatarImage =
                postNode.Descendants("dd")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("title"))
                    .Descendants("img")
                    .FirstOrDefault();

            if (avatarTitle != null)
            {
                user.AvatarTitle = WebUtility.HtmlDecode(avatarTitle.InnerText).WithoutNewLines().Trim();
            }
            if (avatarImage != null)
            {
                user.AvatarLink = avatarImage.GetAttributeValue("src", string.Empty);
            }

            if (avatarTitle != null)
            {
                var testHtml = avatarTitle.InnerHtml;
                testHtml = testHtml.Replace("bbc-center", "");
                if (avatarImage != null) testHtml = testHtml.Replace(avatarImage.OuterHtml, "");
                user.AvatarHtml = testHtml;
            }
            var userIdNode = postNode.DescendantsAndSelf("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("userinfo")) ??
                             postNode.DescendantsAndSelf("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("userinfo"));
            if (userIdNode == null) return;

            var splitString = userIdNode
                .GetAttributeValue("class", string.Empty)
                .Split('-');

            try
            {
                if (splitString.Length >= 2)
                {
                    user.Id =
                        Convert.ToInt64(splitString[1]);
                }
            }
            catch (Exception)
            {

            }
            if (user.Id <= 0)
            {
                if (splitString.Length >= 3)
                {
                    user.Id =
                        Convert.ToInt64(splitString[2]);
                }
            }
            // Remove the UserInfo node after we are done with it, because
            // some forums (FYAD) use it in the body of posts. Why? Who knows!11!1
            userIdNode.Remove();
        }
    }
}
