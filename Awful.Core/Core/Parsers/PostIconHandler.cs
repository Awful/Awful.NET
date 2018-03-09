using Awful.Models.PostIcons;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awful.Parsers
{
    public class PostIconHandler
    {
        public static void Parse(List<PostIconCategory> postIconCategoryList, string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode[] pageNodes = doc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", string.Empty).Equals("posticon")).ToArray();
            var postIconEntityList = new List<PostIcon>();
            foreach (var pageNode in pageNodes)
            {
                var postIconEntity = new PostIcon();
                PostIconHandler.ParsePostIcon(postIconEntity, pageNode);
                postIconEntityList.Add(postIconEntity);
            }
            var postIconCategoryEntity = new PostIconCategory("Post Icon", postIconEntityList);
            postIconCategoryList = new List<PostIconCategory> { postIconCategoryEntity };
        }

        public static void ParsePostIcon(PostIcon postIconEntity, HtmlNode node)
        {
            try
            {
                postIconEntity.Id = Convert.ToInt32(node.Descendants("input").First().GetAttributeValue("value", string.Empty));
                var imageUrl = node.Descendants("img").First().GetAttributeValue("src", string.Empty);
                postIconEntity.ImageUrl = imageUrl;
                postIconEntity.Title = node.Descendants("img").First().GetAttributeValue("alt", string.Empty);
            }
            catch (Exception)
            {
                // If, for some reason, it fails to get an icon, ignore the error.
                // The list view won't show it.
            }
        }
    }
}
