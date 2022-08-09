// <copyright file="PrivateMessageManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageManager"/> class.
        /// </summary>
        /// <param name="webManager">Awful Client.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public PrivateMessageManager(AwfulClient webManager)
        {
            this.webManager = webManager;
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
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var list = PrivateMessageHandler.ParseList(htmlResult.Document);
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

            var result = await this.webManager.GetDataAsync(EndPoints.PrivateMessages + $"?action=show&privatemessageid={id}", false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var body = htmlResult.Document.Body;
                if (body is null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find body.", new Awful.Core.Entities.SAItem(result));
                }

                var post = PostHandler.ParsePost(htmlResult.Document, body);
                post.Result = result;
                return post;
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
                { new StringContent(newPrivateMessageEntity.Title.HtmlEncode()), "title" },
                { new StringContent(newPrivateMessageEntity.Body.HtmlEncode()), "message" },
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
