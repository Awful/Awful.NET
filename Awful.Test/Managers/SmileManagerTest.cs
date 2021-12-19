// <copyright file="SmileManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Xunit;

namespace Awful.Test.Managers
{
    /// <summary>
    /// SmileManager Test.
    /// </summary>
    public class SmileManagerTest
    {
        private readonly Core.ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmileManagerTest"/> class.
        /// </summary>
        public SmileManagerTest()
        {
            this.logger = new Core.DebuggerLogger();
        }

        /// <summary>
        /// Test GetSmileListAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetSmileListAsyncTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            SmileManager manager = new SmileManager(webClient, this.logger);
            var result = await manager.GetSmileListAsync().ConfigureAwait(false);
            Assert.NotNull(result);
            Assert.True(result.SmileCategories.Any());
        }
    }
}
