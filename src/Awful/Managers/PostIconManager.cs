// <copyright file="PostIconManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using Awful.Entities.PostIcons;
using Awful.Entities.Web;
using Awful.Exceptions;
using Awful.Handlers;
using Awful.Utilities;

namespace Awful.Managers
{
    /// <summary>
    /// Manager for Post Icons on Something Awful.
    /// </summary>
    public class PostIconManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostIconManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public PostIconManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Gets post icons for Threads.
        /// </summary>
        /// <param name="forumId">The Forum Id.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of PostIcon's for Threads.</returns>
        public async Task<PostIconList> GetForumPostIconsAsync(int forumId, CancellationToken token = default)
        {
            return await this.GetPostIcons_InternalAsync(false, forumId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets post icons for Private Messages.
        /// </summary>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A list of PostIcon's that can be used for Private Messages.</returns>
        public async Task<PostIconList> GetPrivateMessagePostIconsAsync(CancellationToken token = default)
        {
            return await this.GetPostIcons_InternalAsync(true, 0, token).ConfigureAwait(false);
        }

        private async Task<PostIconList> GetPostIcons_InternalAsync(bool isPrivateMessage = false, int forumId = 0, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Resources.ExceptionMessages.UserAuthenticationError);
            }

            string url = isPrivateMessage ? EndPoints.NewPrivateMessageBase : string.Format(CultureInfo.InvariantCulture, EndPoints.NewThread, forumId);
            var result = await this.webManager.GetDataAsync(url, false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Entities.SAItem(result));
            }

            try
            {
                var icons = PostIconHandler.ParsePostIconList(htmlResult.Document);
                var iconList = new PostIconList(icons) { Result = result };
                return iconList;
            }
            catch (Exception ex)
            {
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
            }
        }
    }
}
