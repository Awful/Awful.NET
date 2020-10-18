// <copyright file="ThreadManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Awful.Exceptions;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Threads on Something Awful.
    /// </summary>
    public class ThreadManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public ThreadManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Mark a thread as 'Unread'.
        /// </summary>
        /// <param name="threadId">The Thread Id.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Task.</returns>
        public async Task<Result> MarkThreadUnreadAsync(long threadId, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var dic = new Dictionary<string, string>
            {
                ["json"] = "1",
                ["action"] = "resetseen",
                ["threadid"] = threadId.ToString(CultureInfo.InvariantCulture),
            };
            using var header = new FormUrlEncodedContent(dic);
            return await this.webManager.PostDataAsync(EndPoints.ShowThreadBase, header, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a new thread cookies. Needed to make a new thread.
        /// </summary>
        /// <param name="forumId">The forum id for where to make the thread.</param>
        /// <param name="token">The CancellationToken.</param>
        /// <returns>A NewThread.</returns>
        public async Task<NewThread> GetThreadCookiesAsync(int forumId, CancellationToken token = default)
        {
            string url = string.Format(CultureInfo.InvariantCulture, EndPoints.NewThread, forumId);
            var result = await this.webManager.GetDataAsync(url, token).ConfigureAwait(false);
            var document = await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false);
            return ThreadHandler.ParseNewThread(document);
        }

        /// <summary>
        /// Creates a new SA Thread.
        /// </summary>
        /// <param name="newThreadEntity">A New Thread Entity.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A result.</returns>
        public async Task<Result> CreateNewThreadAsync(NewThread newThreadEntity, CancellationToken token = default)
        {
            if (newThreadEntity == null)
            {
                throw new ArgumentNullException(nameof(newThreadEntity));
            }

            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("postthread"), "action" },
                { new StringContent(newThreadEntity.ForumId.ToString(CultureInfo.InvariantCulture)), "forumid" },
                { new StringContent(newThreadEntity.FormKey), "formkey" },
                { new StringContent(newThreadEntity.FormCookie), "form_cookie" },
                { new StringContent(newThreadEntity.PostIcon.Id.ToString(CultureInfo.InvariantCulture)), "iconid" },
                { new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Subject)), "subject" },
                { new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Content)), "message" },
                { new StringContent(newThreadEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("Submit Reply"), "submit" },
            };
            return await this.webManager.PostFormDataAsync(EndPoints.NewThreadBase, form, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new thread preview.
        /// </summary>
        /// <param name="newThreadEntity">New Thread Entity.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A post.</returns>
        public async Task<Post> CreateNewThreadPreviewAsync(NewThread newThreadEntity, CancellationToken token = default)
        {
            if (newThreadEntity == null)
            {
                throw new ArgumentNullException(nameof(newThreadEntity));
            }

            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            // We post to SA the same way we would for a normal reply, but instead of getting a redirect back to the
            // thread, we'll get redirected to back to the reply screen with the preview message on it.
            // From here we can parse that preview and return it to the user.
            using var form = new MultipartFormDataContent
            {
                { new StringContent("postthread"), "action" },
                { new StringContent(newThreadEntity.ForumId.ToString(CultureInfo.InvariantCulture)), "forumid" },
                { new StringContent(newThreadEntity.FormKey), "formkey" },
                { new StringContent(newThreadEntity.FormCookie), "form_cookie" },
                { new StringContent(newThreadEntity.PostIcon.Id.ToString(CultureInfo.InvariantCulture)), "iconid" },
                { new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Subject)), "subject" },
                { new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Content)), "message" },
                { new StringContent(newThreadEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("Submit Post"), "submit" },
                { new StringContent("Preview Post"), "preview" },
            };

            var result = await this.webManager.PostFormDataAsync(EndPoints.NewThreadBase, form, token).ConfigureAwait(false);
            return PostHandler.ParsePostPreview(await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false));
        }

        /// <summary>
        /// Marks post as last read.
        /// </summary>
        /// <param name="threadId">The thread id.</param>
        /// <param name="index">The post number index.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Result.</returns>
        public async Task<Result> MarkPostAsLastReadAsAsync(long threadId, int index, CancellationToken token = default)
        {
            return await this.webManager.GetDataAsync(string.Format(CultureInfo.InvariantCulture, EndPoints.LastRead, index, threadId), token).ConfigureAwait(false);
        }
    }
}
