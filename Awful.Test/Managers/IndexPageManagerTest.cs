// <copyright file="IndexPageManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// Tests for the Index Page Manager.
    /// </summary>
    public class IndexPageManagerTest
    {
        /// <summary>
        /// Test GetSortedIndexPageAsync with an unauthenticated user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetSortedIndexPageAsyncUnauthedTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Unauthenticated).ConfigureAwait(false);
            IndexPageManager indexManager = new IndexPageManager(webClient);
            var result = await indexManager.GetSortedIndexPageAsync().ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.User.Userid == 0);
            Assert.True(result.Forums.Any());
            Assert.True(result.Forums.TrueForAll(n => n.Id != 0));
        }

        /// <summary>
        /// Test GetSortedIndexPageAsync with an authenticated user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetSortedIndexPageAsyncAuthedTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            IndexPageManager indexManager = new IndexPageManager(webClient);
            var result = await indexManager.GetSortedIndexPageAsync().ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.User.Userid != 0);
            Assert.True(result.Forums.Any());
            Assert.True(result.Forums.TrueForAll(n => n.Id != 0));
        }
    }
}
