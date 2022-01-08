// <copyright file="StringExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;

namespace Awful.Core.Utilities
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Parses a query string for a given URL.
        /// </summary>
        /// <param name="s">The URL or query string to be parsed.</param>
        /// <returns>A key value dictionary.</returns>
        public static Dictionary<string, string> ParseQueryString(this string s)
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
