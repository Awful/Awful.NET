// <copyright file="HttpClientHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Awful.Core.Utilities
{
    public class HttpClientHelpers
    {
        public static async Task<string> ReadHtmlAsync(HttpResponseMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var stream = await message.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            string html = reader.ReadToEnd();
            return html;
        }
    }
}
