﻿// <copyright file="BookmarkManager.cs" company="Drastic Actions">
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
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Awful.Exceptions;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Bookmark Manager.
    /// </summary>
    public class BookmarkManager
    {
        private readonly AwfulClient webManager;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public BookmarkManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all the bookmarks for a given authenticated user.
        /// </summary>
        /// <param name="perPage">Amount of bookmarked threads to gather, default is 40.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>List of Threads.</returns>
        public async Task<ThreadList> GetAllBookmarksAsync(int perPage = 40, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var threadList = new List<Entities.Threads.Thread>();
            var page = 1;
            while (true)
            {
                var threads = await this.GetBookmarkListAsync(page, perPage, token).ConfigureAwait(false);
                if (!threads.Threads.Any())
                {
                    break;
                }

                threadList.AddRange(threads.Threads);
                page++;
            }

            return new ThreadList(threadList);
        }

        /// <summary>
        /// Gets a single page of Bookmarked Threads for the authenticated user.
        /// </summary>
        /// <param name="page">The bookmark page number.</param>
        /// <param name="perPage">Amount of bookmarked threads to gather, default is 40.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>List of Threads.</returns>
        public async Task<ThreadList> GetBookmarkListAsync(int page, int perPage = 40, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            string url = EndPoints.BookmarksUrl;
            if (page >= 0)
            {
                url = string.Format(CultureInfo.InvariantCulture, EndPoints.BookmarksUrl, perPage) + string.Format(CultureInfo.InvariantCulture, EndPoints.PageNumber, page);
            }

            var result = await this.webManager.GetDataAsync(url, false, token).ConfigureAwait(false);
            if (result?.Document == null)
            {
                throw new Exceptions.AwfulParserException("Failed to find document while getting bookmark thread list page.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var threads = ThreadHandler.ParseForumThreadList(result.Document);
                var threadList = new ThreadList(threads) { CurrentPage = page, Result = result };
                return threadList;
            }
            catch (Exception ex)
            {
                throw new Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Add a new bookmark.
        /// </summary>
        /// <param name="threadId">The Thread Id.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A SA Request Result.</returns>
        public async Task<SAItem> AddBookmarkAsync(int threadId, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var dic = new Dictionary<string, string>
            {
                ["json"] = "1",
                ["action"] = "add",
                ["threadid"] = threadId.ToString(CultureInfo.InvariantCulture),
            };
            using var header = new FormUrlEncodedContent(dic);
            var result = await this.webManager.PostDataAsync(EndPoints.Bookmark, header, true, token).ConfigureAwait(false);
            return new SAItem(result);
        }

        /// <summary>
        /// Removes a bookmark.
        /// </summary>
        /// <param name="threadId">The Thread Id.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A SA Request Result.</returns>
        public async Task<SAItem> RemoveBookmarkAsync(int threadId, CancellationToken token = default)
        {
            if (!this.webManager.IsAuthenticated)
            {
                throw new UserAuthenticationException(Awful.Core.Resources.ExceptionMessages.UserAuthenticationError);
            }

            var dic = new Dictionary<string, string>
            {
                ["json"] = "1",
                ["action"] = "remove",
                ["threadid"] = threadId.ToString(CultureInfo.InvariantCulture),
            };
            using var header = new FormUrlEncodedContent(dic);
            var result = await this.webManager.PostDataAsync(EndPoints.Bookmark, header, true, token).ConfigureAwait(false);
            return new SAItem(result);
        }
    }
}
