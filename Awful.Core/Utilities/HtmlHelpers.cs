// <copyright file="HtmlHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Awful.Core.Utilities
{
    /// <summary>
    /// HTML Helper Methods.
    /// </summary>
    public static class HtmlHelpers
    {
        /// <summary>
        /// Special HTML Encode method to account for weird stuff Something Awful does to strings.
        /// </summary>
        /// <param name="text">String to be HTML encoded.</param>
        /// <returns>HTML Encoded string.</returns>
        public static string HtmlEncode(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // In order to get Unicode characters fully working, we need to first encode the entire post.
            // THEN we decode the bits we can safely pass in, like single/double quotes.
            // If we don't, the post format will be screwed up.
            char[] chars = WebUtility.HtmlEncode(text).ToCharArray();
            var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, "&#{0};", value);
                }
                else
                {
                    result.Append(c);
                }
            }

            result.Replace("&quot;", "\"");
            result.Replace("&#39;", @"'");
            result.Replace("&lt;", @"<");
            result.Replace("&gt;", @">");
            return result.ToString();
        }

        /// <summary>
        /// Parses a query string for a given URL.
        /// </summary>
        /// <param name="s">The URL or query string to be parsed.</param>
        /// <returns>A key value dictionary.</returns>
        public static Dictionary<string, string> ParseQueryString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

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
