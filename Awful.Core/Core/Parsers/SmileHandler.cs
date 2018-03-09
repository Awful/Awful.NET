using Awful.Models.Smilies;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;

namespace Awful.Parsers
{
    public class SmileHandler
    {
        public static void Parse(List<SmileCategory> smileCategoryList, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            IEnumerable<HtmlNode> smileCategoryTitles =
                doc.DocumentNode.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("inner"))
                    .Descendants("h3");
            List<string> categoryTitles =
                smileCategoryTitles.Select(smileCategoryTitle => WebUtility.HtmlDecode(smileCategoryTitle.InnerText))
                    .ToList();
            IEnumerable<HtmlNode> smileNodes =
                doc.DocumentNode.Descendants("ul")
                    .Where(node => node.GetAttributeValue("class", string.Empty).Contains("smilie_group"));
            int smileCount = 0;
            foreach (HtmlNode smileNode in smileNodes)
            {
                var smileList = new List<Smile>();
                IEnumerable<HtmlNode> smileIcons = smileNode.Descendants("li");
                foreach (HtmlNode smileIcon in smileIcons)
                {
                    var smileEntity = new Smile();
                    smileEntity.Parse(smileIcon);
                    smileList.Add(smileEntity);
                }
                smileCategoryList.Add(new SmileCategory()
                {
                    Name = categoryTitles[smileCount],
                    SmileList = smileList
                });
                smileCount++;
            }
        }
    }
}
