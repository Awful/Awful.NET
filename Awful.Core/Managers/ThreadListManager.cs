// <copyright file="ThreadListManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
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
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadListManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        /// <param name="logger">Logger.</param>
        public ThreadListManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
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
            try
            {
                if (result?.Document == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find document while getting forum thread list page.", new Awful.Core.Entities.SAItem(result));
                }

                var threadList = new ThreadList();
                ForumHandler.GetForumPageInfo(result.Document, threadList, this.logger);
                threadList.Threads.AddRange(ThreadHandler.ParseForumThreadList(result.Document));
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
