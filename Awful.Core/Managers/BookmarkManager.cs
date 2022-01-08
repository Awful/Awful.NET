﻿// <copyright file="BookmarkManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities;
using Awful.Core.Entities.Forums;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using Awful.Exceptions;
using System.Globalization;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Bookmark Manager.
    /// </summary>
    public class BookmarkManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public BookmarkManager(AwfulClient webManager)
        {
            this.webManager = webManager;
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

            return new ThreadList(1, 1, new Forum(0, "Bookmarks"), threadList);
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
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document while getting bookmark thread list page.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var (currentPage, totalPages) = CommonHandlers.GetCurrentPageAndTotalPagesFromSelector(htmlResult.Document);
                var threads = ThreadHandler.ParseForumThreadList(htmlResult.Document);
                var threadList = new ThreadList(currentPage, totalPages, new Forum(0, "Bookmarks"), threads) { Result = result };
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