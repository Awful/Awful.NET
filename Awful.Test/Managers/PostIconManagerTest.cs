// <copyright file="PostIconManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// Tests for the post icon manager.
    /// </summary>
    public class PostIconManagerTest
    {
        /// <summary>
        /// Test GetForumPostIconsAsyncTest.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetForumPostIconsAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            PostIconManager manager = new PostIconManager(webClient);
            var result = await manager.GetForumPostIconsAsync(273).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.Icons.Any());
        }

        /// <summary>
        /// Test GetPrivateMessagePostIconsAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPrivateMessagePostIconsAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            PostIconManager manager = new PostIconManager(webClient);
            var result = await manager.GetPrivateMessagePostIconsAsync().ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.Icons.Any());
        }
    }
}
