// <copyright file="ActionsTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.Webview;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Awful.Test.UI
{
    /// <summary>
    /// Test Awful.UI Actions.
    /// </summary>
    public class ActionsTest
    {
        private TemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsTest"/> class.
        /// </summary>
        public ActionsTest()
        {
            this.templates = new TemplateHandler();
        }

        /// <summary>
        /// Test SigninAction.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SigninActionTest()
        {
            var properties = new TestPlatformProperties("signin");
            using var context = new AwfulContext(properties);
            var signinAction = new SigninAction(properties, context);
            signinAction.OnSignin += this.SigninAction_OnSignin;
            var userauth = await signinAction.SigninAsync(Environment.GetEnvironmentVariable("AWFUL_USER"), Environment.GetEnvironmentVariable("AWFUL_PASSWORD")).ConfigureAwait(false);
            Assert.True(userauth.IsSuccess);
            Assert.NotNull(userauth.CurrentUser);
            Assert.True(await context.Users.AnyAsync().ConfigureAwait(false));
            signinAction.OnSignin -= this.SigninAction_OnSignin;
        }

        /// <summary>
        /// Test SAsclopediaAction.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SAclopediaActionTest()
        {
            var properties = new TestPlatformProperties("saclopedia");
            using var context = new AwfulContext(properties);
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            var saclopediaAction = new SAclopediaAction(webClient, context, this.templates);
            var entries = await saclopediaAction.LoadSAclopediaEntryItemsAsync().ConfigureAwait(false);
            Assert.NotNull(entries);
            Assert.True(entries.Any());

            // Should use cache.
            entries = await saclopediaAction.LoadSAclopediaEntryItemsAsync(false).ConfigureAwait(false);
            Assert.NotNull(entries);
            Assert.True(entries.Any());

            // Should refresh.
            entries = await saclopediaAction.LoadSAclopediaEntryItemsAsync(true).ConfigureAwait(false);
            Assert.NotNull(entries);
            Assert.True(entries.Any());

            var entry = await saclopediaAction.LoadSAclopediaEntryAsync(entries.First()).ConfigureAwait(false);
            Assert.NotNull(entry);
            Assert.True(entry.Posts.Any());

            var html = saclopediaAction.GenerateSAclopediaEntryTemplate(entry, new Webview.Entities.Themes.DefaultOptions());
            Assert.True(!string.IsNullOrEmpty(html));
        }

        private void SigninAction_OnSignin(object sender, Awful.UI.Events.SigninEventArgs e)
        {
            Assert.True(e.IsSignedIn);
            Assert.NotNull(e);
        }
    }
}
