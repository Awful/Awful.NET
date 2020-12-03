// <copyright file="BookmarkManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// Tests for Bookmark Manager.
    /// </summary>
    public class BookmarkManagerTest
    {
        /// <summary>
        /// Test GetAllBookmarksAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetAllBookmarksAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            BookmarkManager manager = new BookmarkManager(webClient);
            var result = await manager.GetAllBookmarksAsync().ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.Threads.Any());
        }

        /// <summary>
        /// Test GetBookmarkListAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetBookmarkListAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            BookmarkManager manager = new BookmarkManager(webClient);
            var result = await manager.GetBookmarkListAsync(1).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.Threads.Any());
        }
    }
}
