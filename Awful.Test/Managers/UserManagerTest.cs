// <copyright file="UserManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// UserManager Test.
    /// </summary>
    public class UserManagerTest
    {
        /// <summary>
        /// Test GetUserFromProfilePageAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserFromProfilePageAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            UserManager manager = new UserManager(webClient);

            // 0 = default user.
            var user = await manager.GetUserFromProfilePageAsync(0).ConfigureAwait(false);
            Assert.NotNull(user);
            Assert.NotEmpty(user.Username);
            Assert.Equal(Environment.GetEnvironmentVariable("AWFUL_USER"), user.Username);
        }
    }
}
