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
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsTest"/> class.
        /// </summary>
        public ActionsTest()
        {
            this.templates = new MobileTemplateHandler();
        }

        /// <summary>
        /// Test SigninAction.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SigninActionTest()
        {
            var properties = new TestPlatformProperties("signin");
            using var context = new AwfulSqliteContext(properties);
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
            using var context = new AwfulSqliteContext(properties);
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

        /// <summary>
        /// Test Bookmark Action.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task BookmarkActionTest()
        {
            var properties = new TestPlatformProperties("bookmarks");
            using var context = new AwfulSqliteContext(properties);
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            var bookmarkAction = new BookmarkAction(webClient, context);
            var bookmarks = await bookmarkAction.GetAllBookmarksAsync().ConfigureAwait(false);
            Assert.NotNull(bookmarks);
            Assert.True(bookmarks.Any());

            bookmarks = await bookmarkAction.AddBookmarkAsync(3908659).ConfigureAwait(false);
            Assert.NotNull(bookmarks);
            Assert.True(bookmarks.Any());
            Assert.Contains(bookmarks, n => n.ThreadId == 3908659);

            bookmarks = await bookmarkAction.RemoveBookmarkAsync(3908659).ConfigureAwait(false);
            Assert.NotNull(bookmarks);
            Assert.True(bookmarks.Any());
            Assert.DoesNotContain(bookmarks, n => n.ThreadId == 3908659);
        }

        /// <summary>
        /// Test Setting Actions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SettingsActionTest()
        {
            var properties = new TestPlatformProperties("settings");
            using var context = new AwfulSqliteContext(properties);
            var settingAction = new SettingsAction(context);

            var settings = await settingAction.LoadSettingOptionsAsync().ConfigureAwait(false);
            Assert.NotNull(settings);
            settings.EnableBackgroundTasks = true;
            settings = await settingAction.SaveSettingOptionsAsync(settings).ConfigureAwait(false);
            Assert.True(settings.EnableBackgroundTasks);
        }

        /// <summary>
        /// Test BanActions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task BanActionsTest()
        {
            var properties = new TestPlatformProperties("ban");
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            using var probationClient = await Setup.SetupWebClient(AwfulUser.Probation).ConfigureAwait(false);
            using var context = new AwfulSqliteContext(properties);
            var banActions = new BanActions(webClient, context, this.templates);
            var proBanActions = new BanActions(probationClient, context, this.templates);
            var entry = await banActions.GetBanPageAsync().ConfigureAwait(false);
            Assert.NotNull(entry);
            var htmlString = banActions.RenderBanView(entry, new Webview.Entities.Themes.DefaultOptions());
            Assert.NotEmpty(htmlString);

            var notProbation = await banActions.CheckForProbationAsync().ConfigureAwait(false);
            Assert.NotNull(notProbation);
            Assert.False(notProbation.IsUnderProbation);

            var isUnderProbation = await proBanActions.CheckForProbationAsync().ConfigureAwait(false);
            Assert.NotNull(isUnderProbation);
            Assert.True(isUnderProbation.IsUnderProbation);
        }

        /// <summary>
        /// Test UserActions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UserActionsTest()
        {
            var properties = new TestPlatformProperties("user");
            using var webClient = await Setup.SetupWebClient(AwfulUser.Standard).ConfigureAwait(false);
            using var context = new AwfulSqliteContext(properties);
            var userActions = new UserActions(webClient, context, this.templates);

            var user = await userActions.GetLoggedInUserAsync().ConfigureAwait(false);
            Assert.NotNull(user);
            Assert.NotEmpty(user.Username);
            Assert.Equal(Environment.GetEnvironmentVariable("AWFUL_USER"), user.Username);

            var userProfile = userActions.RenderProfileView(user, new Webview.Entities.Themes.DefaultOptions());
            Assert.NotEmpty(userProfile);
        }

        /// <summary>
        /// Test ThreadPostActions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ThreadPostActions()
        {
            var properties = new TestPlatformProperties("threadpost");
            using var webClient = await Setup.SetupWebClient(AwfulUser.Platinum).ConfigureAwait(false);
            using var context = new AwfulSqliteContext(properties);
            var tpActions = new ThreadPostActions(webClient, context, this.templates);
            var thread = await tpActions.GetThreadPostsAsync(3606621).ConfigureAwait(false);
            Assert.NotNull(thread);
            Assert.True(thread.Posts.Any());

            var threadHtml = tpActions.RenderThreadPostView(thread, new Webview.Entities.Themes.DefaultOptions());
            Assert.NotEmpty(threadHtml);
        }

        private void SigninAction_OnSignin(object sender, Awful.UI.Events.SigninEventArgs e)
        {
            Assert.True(e.IsSignedIn);
            Assert.NotNull(e);
        }
    }
}
