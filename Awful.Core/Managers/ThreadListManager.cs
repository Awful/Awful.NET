// <copyright file="ThreadListManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Thread Lists on Something Awful.
    /// </summary>
    public class ThreadListManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadListManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public ThreadListManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Gets the list of threads in a given Forum.
        /// </summary>
        /// <param name="forumId">The Forum Id.</param>
        /// <param name="page">The page of the forum to get.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A ThreadList.</returns>
        public async Task<ThreadList> GetForumThreadListAsync(int forumId, int page, CancellationToken token = default)
        {
            var pageUrl = string.Format(CultureInfo.InvariantCulture, EndPoints.ForumPage, forumId, EndPoints.DefaultNumberPerPage) + string.Format(CultureInfo.InvariantCulture, EndPoints.PageNumber, page);
            var result = await this.webManager.GetDataAsync(pageUrl, false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var forum = ForumHandler.GetForumInfoViaThreadListPage(htmlResult.Document);
                var threads = ThreadHandler.ParseForumThreadList(htmlResult.Document);
                var (currentPage, totalPages) = CommonHandlers.GetCurrentPageAndTotalPagesFromSelector(htmlResult.Document);
                var threadList = new ThreadList(currentPage, totalPages, forum, threads);
                threadList.Result = result;
                return threadList;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
