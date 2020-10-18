// <copyright file="BanManagerTest.cs" company="Drastic Actions">
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
    /// Ban Manager Test.
    /// </summary>
    public class BanManagerTest
    {
        /// <summary>
        /// Assert Probated User is discovered.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CheckForProbationWithProbatedUser()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Probation).ConfigureAwait(false);
            BanManager banManager = new BanManager(webClient);
            var banResult = await banManager.CheckForProbationAsync().ConfigureAwait(false);
            Assert.True(banResult.IsUnderProbation);
        }

        /// <summary>
        /// Assert Probated User is discovered.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CheckForProbationWithNonProbatedUser()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            BanManager banManager = new BanManager(webClient);
            var banResult = await banManager.CheckForProbationAsync().ConfigureAwait(false);
            Assert.False(banResult.IsUnderProbation);
        }

        /// <summary>
        /// Assert GetBanPageAsync returns proper results.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetBanPageAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            BanManager banManager = new BanManager(webClient);
            var banResult = await banManager.GetBanPageAsync().ConfigureAwait(false);
            Assert.True(banResult.Bans.Any());
            Assert.True(banResult.TotalPages > 1);
        }
    }
}
