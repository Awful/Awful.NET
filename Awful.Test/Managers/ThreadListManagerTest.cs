﻿// <copyright file="ThreadListManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core;
using Awful.Core.Managers;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// Tests for Thread List Manager.
    /// </summary>
    public class ThreadListManagerTest
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadListManagerTest"/> class.
        /// </summary>
        public ThreadListManagerTest()
        {
            this.logger = new DebuggerLogger();
        }

        /// <summary>
        /// Test GetForumThreadListAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetForumThreadListAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            ThreadListManager manager = new ThreadListManager(webClient, this.logger);
            var result = await manager.GetForumThreadListAsync(273, 1).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.CurrentPage == 1);
            Assert.True(result.TotalPages > 1);
            Assert.True(result.Threads.Any());
        }
    }
}
