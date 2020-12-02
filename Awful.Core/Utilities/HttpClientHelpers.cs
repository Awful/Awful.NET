// <copyright file="HttpClientHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Awful.Core.Utilities
{
    /// <summary>
    /// Http Client Helpers.
    /// </summary>
    public static class HttpClientHelpers
    {
        /// <summary>
        /// Read Html out of <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="message"><see cref="HttpResponseMessage"/></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
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
