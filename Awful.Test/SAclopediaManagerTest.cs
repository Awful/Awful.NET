// <copyright file="SAclopediaManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Xunit;

namespace Awful.Test
{
    /// <summary>
    /// SAclopedia Manager Tests.
    /// </summary>
    public class SAclopediaManagerTest
    {
        /// <summary>
        /// Parse SAclopedia Entries.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SAclopediaTest()
        {
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            SAclopediaManager manager = new SAclopediaManager(webClient);
            var categoryList = await manager.GetCategoryListAsync().ConfigureAwait(false);
            Assert.True(categoryList.Any());
            var entryList = await manager.GetEntryItemListAsync(categoryList.First().Id).ConfigureAwait(false);
            Assert.True(entryList.Any());
            var entry = await manager.GetEntryAsync(entryList.First().Id).ConfigureAwait(false);
            Assert.True(entry.Posts.Any());
        }
    }
}
