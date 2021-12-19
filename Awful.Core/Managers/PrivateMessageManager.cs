﻿// <copyright file="PrivateMessageManager.cs" company="Drastic Actions">
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
using Awful.Core.Entities;
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
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageManager"/> class.
        /// </summary>
        /// <param name="webManager">Awful Client.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public PrivateMessageManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
        }

        /// <summary>
        /// Get all private messages.
        /// </summary>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>List of Private Messages.</returns>
        public async Task<PrivateMessageList> GetAllPrivateMessageListAsync(CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var url = EndPoints.PrivateMessages;
            var result = await this.webManager.GetDataAsync(url, false, token).ConfigureAwait(false);
            try
            {
                var list = PrivateMessageHandler.ParseList(result.Document);
                var pmList = new PrivateMessageList(list) { Result = result };
                return pmList;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
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

            var message = new PrivateMessage() { PrivateMessageId = id };
            var result = await this.webManager.GetDataAsync(EndPoints.PrivateMessages + $"?action=show&privatemessageid={message.PrivateMessageId}", false, token).ConfigureAwait(false);
            try
            {
                message.Result = result;
                message.Post = PostHandler.ParsePost(result.Document, result.Document.Body);
                return message.Post;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Send Private Message.
        /// </summary>
        /// <param name="newPrivateMessageEntity">New PM.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<SAItem> SendPrivateMessageAsync(NewPrivateMessage newPrivateMessageEntity, CancellationToken token = default)
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

            var result = await this.webManager.PostFormDataAsync(EndPoints.NewPrivateMessageBase, form, false, token).ConfigureAwait(false);
            return new SAItem(result);
        }
    }
}
