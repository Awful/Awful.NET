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
        /// <summary>
        /// Test CreateNewThreadAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CreateNewThreadAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            PrivateMessageManager manager = new PrivateMessageManager(webClient);
            var pm = new NewPrivateMessage();
            PostIconManager iconManager = new PostIconManager(webClient);
            var iconResult = await iconManager.GetPrivateMessagePostIconsAsync().ConfigureAwait(false);
            Assert.NotNull(iconResult);
            var date = DateTime.UtcNow;
            pm.Body = $"Awful.NET Unit Test PM Create: {date}";
            pm.Title = $"Awful.NET Unit Test PM Create: {date}";
            pm.Receiver = Environment.GetEnvironmentVariable("AWFUL_PLATINUM_USER");
            pm.Icon = iconResult.Icons.First();
            var result = await manager.SendPrivateMessageAsync(pm).ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Result.ResultText);
        }
    }
}
