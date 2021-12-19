// <copyright file="ThreadReplyManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// Tests for ThreadReplyManager.
    /// </summary>
    public class ThreadReplyManagerTest
    {
        private readonly Core.ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyManagerTest"/> class.
        /// </summary>
        public ThreadReplyManagerTest()
        {
            this.logger = new Core.DebuggerLogger();
        }

        /// <summary>
        /// Test ReplyToPost.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReplyToPostTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            var manager = new ThreadReplyManager(webClient, this.logger);
            var threadReply = await manager.GetReplyCookiesAsync(3947346).ConfigureAwait(false);
            Assert.NotNull(threadReply);
            Assert.NotEmpty(threadReply.FormCookie);
            Assert.NotEmpty(threadReply.FormKey);

            threadReply.Message = $"Awful.NET Unit Test Reply: {DateTime.UtcNow}";
            var previewPost = await manager.CreatePreviewPostAsync(threadReply).ConfigureAwait(false);
            Assert.NotNull(previewPost);
            Assert.NotEmpty(previewPost.PostHtml);

            var result = await manager.SendPostAsync(threadReply).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.AbsoluteEndpoint == EndPoints.NewReply);
        }

        [Fact]
        public async Task EditPostTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            var manager = new ThreadReplyManager(webClient, this.logger);
            var threadReply = await manager.GetReplyCookiesForEditAsync(509785640).ConfigureAwait(false);
            Assert.NotNull(threadReply);
            Assert.NotEmpty(threadReply.Message);

            threadReply.Message = $"Awful.NET Unit Test Edit Reply: {DateTime.UtcNow}";
            var previewPost = await manager.CreatePreviewEditPostAsync(threadReply).ConfigureAwait(false);
            Assert.NotNull(previewPost);
            Assert.NotEmpty(previewPost.PostHtml);

            var result = await manager.SendUpdatePostAsync(threadReply).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.AbsoluteEndpoint == EndPoints.EditPost);
        }
    }
}
