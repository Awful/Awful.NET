using AngleSharp.Dom.Html;
using Awful.Parser.Models.SAclopedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awful.Parser.Handlers
{
    public class SAclopediaHandler
    {
        public static List<SAclopediaCategory> ParseCategoryList(IHtmlDocument document)
        {
            return document.QuerySelector(".letternav").QuerySelectorAll("a").Select(n => new SAclopediaCategory() { Letter = n.TextContent.Trim(), Id = Convert.ToInt32(n.GetAttribute("href").Split('=').Last())  }).ToList();
        }

        public static List<SAclopediaEntryItem> ParseEntryItemList(IHtmlDocument document)
        {
            return document.QuerySelector("#topiclist").QuerySelectorAll("a").Select(n => new SAclopediaEntryItem() { Title = n.TextContent.Trim(), Id = Convert.ToInt32(n.GetAttribute("href").Split('=').Last()) }).ToList();
        }

        public static SAclopediaEntry ParseEntry(IHtmlDocument document, int id)
        {
            var saclopediaEntry = new SAclopediaEntry()
            {
                Id = id,
                Title = document.QuerySelector(@"h1[class=""topic""]").TextContent
            };

            var postsLi = document.QuerySelector("#posts").QuerySelectorAll("li");
            foreach(var postli in postsLi)
            {
                var entry = new SAclopediaPost();
                var userThing = postli.QuerySelector(".byline");
                if (userThing != null)
                {
                    entry.Username = userThing.QuerySelector("a").TextContent;
                    entry.UserId = Convert.ToInt32(userThing.QuerySelector("a").GetAttribute("href").Split('=').Last());
                    var htmlTest = userThing.InnerHtml;
                    var lastBracket = htmlTest.LastIndexOf('>');
                    if (lastBracket > 0)
                    {
                        entry.PostedDate = DateTime.Parse(htmlTest.Substring(lastBracket + 5));
                    }
                }

                entry.PostHtml = postli.LastElementChild.InnerHtml;
                saclopediaEntry.Posts.Add(entry);
            }

            return saclopediaEntry;
        }
    }
}
