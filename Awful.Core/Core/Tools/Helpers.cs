using Awful.Models.Messages;
using Awful.Models.PostIcons;
using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Models.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Awful.Tools
{
  
   
    public class PrivateMessageHandler
    {
        public void Parse(PrivateMessage pmEntity, HtmlNode rowNode)
        {
            pmEntity.Status =
                rowNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("status"))
                    .Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", string.Empty);

            var icon = rowNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon"))
                    .Descendants("img")
                    .FirstOrDefault();

            if (icon != null)
            {
                pmEntity.Icon = new Models.PostIcons.PostIcon() { ImageUrl = icon.GetAttributeValue("src", string.Empty) };
                pmEntity.ImageIconLocation = Path.GetFileNameWithoutExtension(icon.GetAttributeValue("src", string.Empty));
            }

            var titleNode = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("title"));

            pmEntity.Title =
               titleNode
                    .InnerText.Replace("\n", string.Empty).Trim();

            string titleHref = titleNode.Descendants("a").FirstOrDefault().GetAttributeValue("href", string.Empty).Replace("&amp;", "&");

            pmEntity.MessageUrl = EndPoints.BaseUrl + titleHref;

            pmEntity.Sender = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("sender"))
                .InnerText;
            pmEntity.Date = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("date"))
                .InnerText;
        }
    }

    public static class ErrorHandler
    {
        public static Result CreateErrorObject(Result result, string reason, string stacktrace, string type = "", bool isPaywall = false)
        {
            result.IsSuccess = false;
            result.Type = typeof(Error).ToString();
            if (!isPaywall)
            {
                isPaywall = reason.Equals("paywall");
            }
            var error = new Error()
            {
                Type = type,
                Reason = reason,
                StackTrace = stacktrace,
                IsPaywall = isPaywall
            };
            result.ResultJson = JsonConvert.SerializeObject(error);
            return result;
        }
    }

    public static class HtmlHelpers
    {
        public static string HtmlEncode(string text)
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

        public static string WithoutNewLines(this string text)
        {
            var sb = new StringBuilder(text.Length);
            foreach (char i in text)
            {
                if (i != '\n' && i != '\r' && i != '\t' && i != '#' && i != '?')
                {
                    sb.Append(i);
                }
                else if (i == '\n')
                {
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }

        public static Dictionary<string, string> ParseQueryString(string s)
        {
            var nvc = new Dictionary<string, string>();

            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }

            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    nvc.Add(singlePair[0], string.Empty);
                }
            }

            return nvc;
        }
    }
}
