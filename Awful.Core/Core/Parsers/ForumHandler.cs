using Awful.Models.Forums;
using Awful.Models.Web;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parsers
{
    public class ForumHandler
    {
        public static List<Category> ParseCategoryList(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var forumGroupList = new List<Category>();
            var forumNode =
                doc.DocumentNode.Descendants("select")
                    .FirstOrDefault(node => node.GetAttributeValue("name", String.Empty).Equals("forumid"));
            if (forumNode == null)
            {
                throw new Exception("Could not download main forum list.");
            }

            try
            {
                var forumNodes = forumNode.Descendants("option");
                var parentId = 0;
                var order = 1;
                foreach (var node in forumNodes)
                {
                    var value = node.Attributes["value"].Value;
                    int id;
                    if (!int.TryParse(value, out id) || id <= -1) continue;
                    if (node.InnerText.Contains("--"))
                    {
                        var forumName =
                            WebUtility.HtmlDecode(node.InnerText.Replace("-", String.Empty)).Trim();
                        var substringText = node.InnerText.Substring(0, 5);
                        var isSubforum = substringText.Cast<char>().Count(c => c == '-') > 2;
                        var forumCategory = forumGroupList.LastOrDefault();

                        var forumSubCategory = new Forum
                        {
                            Name = forumName.Trim(),
                            Location = String.Format(EndPoints.ForumPage, value),
                            IsSubforum = isSubforum,
                            Category = forumCategory,
                            CategoryId = forumCategory.Id
                        };
                        SetForumId(forumSubCategory);
                        if (!isSubforum)
                        {
                            parentId = forumSubCategory.ForumId;
                        }
                        else
                        {
                            // forumSubCategory.ParentForumId = parentId;
                        }

                        forumCategory.ForumList.Add(forumSubCategory);
                    }
                    else
                    {
                        var forumName = WebUtility.HtmlDecode(node.InnerText).Trim();
                        var forumGroup = new Category()
                        {
                            Name = forumName,
                            Location = String.Format(EndPoints.ForumPage, value),
                            Id = Convert.ToInt32(value),
                            Order = order,
                            ForumList = new List<Forum>()
                        };
                        order++;
                        forumGroupList.Add(forumGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Main Forum Parsing Error: " + ex.StackTrace);
            }

#if DEBUG
            if (forumGroupList.Any())
                forumGroupList[3].ForumList.Add(AddDebugForum());
#endif

            return forumGroupList;
        }

        public static Forum AddDebugForum()
        {
            var forum = new Forum()
            {
                Name = "Apps In Developmental States",
                Location = EndPoints.BaseUrl + "forumdisplay.php?forumid=261",
                IsSubforum = false
            };
            SetForumId(forum);
            return forum;
        }

        public static void SetForumId(Forum forumEntity)
        {
            if (String.IsNullOrEmpty(forumEntity.Location))
            {
                forumEntity.ForumId = 0;
                return;
            }

            var forumId = forumEntity.Location.Split('=');
            if (forumId.Length > 1)
            {
                forumEntity.ForumId = Convert.ToInt32(forumEntity.Location.Split('=')[1]);
            }
        }
    }
}
