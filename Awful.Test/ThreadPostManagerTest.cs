// <copyright file="ThreadPostManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Xunit;

namespace Awful.Test
{
    /// <summary>
    /// Tests for Thread List Manager.
    /// </summary>
    public class ThreadPostManagerTest
    {
        /// <summary>
        /// Test GetThreadPostsAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetThreadPostsAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            ThreadPostManager manager = new ThreadPostManager(webClient);
            var result = await manager.GetThreadPostsAsync(3836680, 1).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.CurrentPage == 1);
            Assert.True(result.TotalPages > 1);
            Assert.True(result.Posts.Any());
            Assert.NotNull(result.Posts.First().User);
        }
    }
}