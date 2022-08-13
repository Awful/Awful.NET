// <copyright file="ThreadPostManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using Awful.Entities.Threads;
using Awful.Entities.Web;
using Awful.Handlers;
using Awful.Utilities;

namespace Awful.Managers
{
    /// <summary>
    /// SA Thread Post Manager.
    /// </summary>
    public class ThreadPostManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public ThreadPostManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Gets a thread. Can be used with or without authentication, but depending on the thread it may be behind a paywall.
        /// This should be wrapped to check for <see cref="PaywallException"/>.
        /// </summary>
        /// <param name="threadId">A Thread Id.</param>
        /// <param name="pageNumber">The page number. Defaults to 1.</param>
        /// <param name="goToNewestPost">Goes to the newest page and post in a thread. Overrides pageNumber if set to True.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A Thread.</returns>
        public async Task<ThreadPost> GetThreadPostsAsync(int threadId, int pageNumber = 1, bool goToNewestPost = false, CancellationToken token = default)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            var baseUri = string.Format(CultureInfo.InvariantCulture, EndPoints.ThreadPage, threadId, EndPoints.DefaultNumberPerPage);
            if (goToNewestPost)
            {
                baseUri += string.Format(CultureInfo.InvariantCulture, EndPoints.GotoNewPost);
            }
            else if (pageNumber > 1)
            {
                baseUri += string.Format(CultureInfo.InvariantCulture, EndPoints.PageNumber, pageNumber);
            }

            var result = await this.webManager.GetDataAsync(baseUri, false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Entities.SAItem(result));
            }

            try
            {
                var item = ThreadPostHandler.ParseThread(htmlResult.Document);
                item.Result = result;
                return item;
            }
            catch (Exception ex)
            {
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
            }
        }
    }
}
