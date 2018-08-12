using AngleSharp.Dom.Html;
using Awful.Parser.Models.PostIcons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Awful.Parser.Handlers
{
    public class PostIconHandler
    {
        public static List<PostIcon> ParsePostIconList(IHtmlDocument document)
        {
            var postIconList = new List<PostIcon>();
            var postIconsHtml = document.QuerySelectorAll(".posticon");
            foreach(var postIconHtml in postIconsHtml)
            {
                var inputId = Convert.ToInt32(postIconHtml.QuerySelector("input").GetAttribute("value"));
                var image = postIconHtml.QuerySelector("img");
                var srcAlt = image.GetAttribute("alt");
                var imageLocation = image.GetAttribute("src");
                postIconList.Add(new PostIcon()
                {
                    Id = inputId,
                    ImageUrl = imageLocation,
                    ImageLocation = Path.GetFileNameWithoutExtension(imageLocation),
                    Title = srcAlt
                });
            }

            return postIconList;
        }
    }
}
