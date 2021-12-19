// <copyright file="ThreadManagerTest.cs" company="Drastic Actions">
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
    /// ThreadManager Test.
    /// </summary>
    public class ThreadManagerTest
    {
        private readonly Core.ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadManagerTest"/> class.
        /// </summary>
        public ThreadManagerTest()
        {
            this.logger = new Core.DebuggerLogger();
        }

        /// <summary>
        /// Test MarkThreadUnreadAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MarkThreadUnreadAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            ThreadManager manager = new ThreadManager(webClient, this.logger);
            var result = await manager.MarkThreadUnreadAsync(3910844).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.NotEmpty(result.ResultText);
        }

        /// <summary>
        /// Test MarkPostAsLastReadAsAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MarkPostAsLastReadAsAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            ThreadManager manager = new ThreadManager(webClient, this.logger);
            var result = await manager.MarkPostAsLastReadAsAsync(3910844, 1).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.NotEmpty(result.ResultText);
        }

        /// <summary>
        /// Test CreateNewThreadAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CreateNewThreadAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            ThreadManager manager = new ThreadManager(webClient, this.logger);
            PostIconManager iconManager = new PostIconManager(webClient, this.logger);
            var iconResult = await iconManager.GetForumPostIconsAsync(261).ConfigureAwait(false);
            Assert.NotNull(iconResult);
            var icon = iconResult.Icons.First(n => n.Title == "Windows");
            var result = await manager.GetThreadCookiesAsync(261).ConfigureAwait(false);
            var date = DateTime.UtcNow;
            result.Content = $"Awful.NET Unit Test Thread Create: {date}";
            result.Subject = $"Awful.NET Unit Test Thread Create: {date}";
            result.PostIcon = icon;
            var preview = await manager.CreateNewThreadPreviewAsync(result).ConfigureAwait(false);
            Assert.NotNull(preview);
            Assert.NotEmpty(preview.PostHtml);

            // We don't want to post a ton of new threads in the live forums.
            // Thread Preview should be enough to show it works.

            // var thread = await manager.CreateNewThreadAsync(result).ConfigureAwait(false);
            // Assert.NotNull(thread);
            // Assert.NotEmpty(thread.ResultHtml);
        }
    }
}
