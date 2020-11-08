// <copyright file="PrivateMessageManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Awful.Exceptions;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Private Message Manager.
    /// </summary>
    public class PrivateMessageManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageManager"/> class.
        /// </summary>
        /// <param name="webManager">Awful Client.</param>
        public PrivateMessageManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Get all private messages.
        /// </summary>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>List of Private Messages.</returns>
        public async Task<List<PrivateMessage>> GetAllPrivateMessageListAsync(CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var pmList = new List<PrivateMessage>();
            var page = 0;
            while (true)
            {
                var result = await this.GetPrivateMessageListAsync(page, token).ConfigureAwait(false);
                pmList.AddRange(result);
                if (!result.Any())
                {
                    break;
                }

                page++;
            }

            return pmList;
        }

        /// <summary>
        /// Get Private Message List per page.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>List of private messages.</returns>
        public async Task<List<PrivateMessage>> GetPrivateMessageListAsync(int page, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var url = EndPoints.PrivateMessages;
            if (page > 0)
            {
                url = EndPoints.PrivateMessages + string.Format(CultureInfo.InvariantCulture, EndPoints.PageNumber, page);
            }

            var result = await this.webManager.GetDataAsync(url, token).ConfigureAwait(false);
            var document = await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false);
            return PrivateMessageHandler.ParseList(document);
        }

        /// <summary>
        /// Get private message.
        /// </summary>
        /// <param name="id">Post Id.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<Post> GetPrivateMessageAsync(int id, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var message = new PrivateMessage() { Id = id };
            var result = await this.webManager.GetDataAsync(EndPoints.PrivateMessages + $"?action=show&privatemessageid={message.Id}", token).ConfigureAwait(false);
            var document = await this.webManager.Parser.ParseDocumentAsync(result.ResultHtml, token).ConfigureAwait(false);
            message.Post = PostHandler.ParsePost(document, document.Body);
            return message.Post;
        }

        /// <summary>
        /// Send Private Message.
        /// </summary>
        /// <param name="newPrivateMessageEntity">New PM.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendPrivateMessageAsync(NewPrivateMessage newPrivateMessageEntity, CancellationToken token = default)
        {
            if (newPrivateMessageEntity == null)
            {
                throw new ArgumentNullException(nameof(newPrivateMessageEntity));
            }

            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("dosend"), "action" },
                { new StringContent(newPrivateMessageEntity.Receiver), "touser" },
                { new StringContent(HtmlHelpers.HtmlEncode(newPrivateMessageEntity.Title)), "title" },
                { new StringContent(HtmlHelpers.HtmlEncode(newPrivateMessageEntity.Body)), "message" },
                { new StringContent("yes"), "parseurl" },
                { new StringContent("yes"), "parseurl" },
                { new StringContent("Send Message"), "submit" },
            };
            if (newPrivateMessageEntity.Icon != null)
            {
                form.Add(new StringContent(newPrivateMessageEntity.Icon.Id.ToString(CultureInfo.InvariantCulture)), "iconid");
            }

            return await this.webManager.PostFormDataAsync(EndPoints.NewPrivateMessageBase, form, token).ConfigureAwait(false);
        }
    }
}
