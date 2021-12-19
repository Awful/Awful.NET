// <copyright file="PrivateMessageManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.Messages;
using Awful.Core.Managers;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// Test PrivateMessageManager.
    /// </summary>
    public class PrivateMessageManagerTest
    {
        private readonly Core.ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageManagerTest"/> class.
        /// </summary>
        public PrivateMessageManagerTest()
        {
            this.logger = new Core.DebuggerLogger();
        }

        /// <summary>
        /// Test CreateNewThreadAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CreateNewThreadAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            PrivateMessageManager manager = new PrivateMessageManager(webClient, this.logger);
            PostIconManager iconManager = new PostIconManager(webClient, this.logger);
            var iconResult = await iconManager.GetPrivateMessagePostIconsAsync().ConfigureAwait(false);
            Assert.NotNull(iconResult);
            var date = DateTime.UtcNow;
            var body = $"Awful.NET Unit Test PM Create: {date}";
            var title = $"Awful.NET Unit Test PM Create: {date}";
            var receiver = Environment.GetEnvironmentVariable("AWFUL_PLATINUM_USER");
            var icon = iconResult.Icons.First();
            var pm = new NewPrivateMessage(icon, title, receiver, body);
            var result = await manager.SendPrivateMessageAsync(pm).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Result.ResultText);
        }
    }
}
