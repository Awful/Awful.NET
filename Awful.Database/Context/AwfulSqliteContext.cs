// <copyright file="AwfulSqliteContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Entities;
using Awful.Webview.Entities.Themes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Awful.Database.Context
{
    /// <summary>
    /// Awful Database Context.
    /// </summary>
    public class AwfulSqliteContext : DbContext, IAwfulContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulSqliteContext"/> class.
        /// </summary>
        /// <param name="platformProperties">Path to the platform properties.</param>
        public AwfulSqliteContext(IPlatformProperties platformProperties)
        {
            this.PlatformProperties = platformProperties;
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets or sets the users table.
        /// </summary>
        public DbSet<UserAuth> Users { get; set; }

        /// <summary>
        /// Gets or sets the forums table.
        /// </summary>
        public DbSet<Forum> Forums { get; set; }

        /// <summary>
        /// Gets or sets the SAclopediaEntryItems table.
        /// </summary>
        public DbSet<SAclopediaEntryItem> SAclopediaEntryItems { get; set; }

        /// <summary>
        /// Gets or sets the SettingOptions table.
        /// </summary>
        public DbSet<SettingOptions> SettingOptionsItems { get; set; }

        /// <summary>
        /// Gets or sets the BookmarkThreads table.
        /// </summary>
        public DbSet<AwfulThread> BookmarkThreads { get; set; }

        /// <summary>
        /// Gets or sets the PrivateMessages table.
        /// </summary>
        public DbSet<AwfulPM> PrivateMessages { get; set; }

        private IPlatformProperties PlatformProperties { get; set; }

        /// <summary>
        /// Resets the local database.
        /// </summary>
        public void ResetDatabase()
        {
            this.GetDependencies().StateManager.ResetState();
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        #region SAclopedia

        /// <summary>
        /// Add All SAclopedia Entries.
        /// </summary>
        /// <param name="entries">Entries to be added.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<int> AddAllSAclopediaEntry(List<SAclopediaEntryItem> entries)
        {
            await this.SAclopediaEntryItems.AddRangeAsync(entries).ConfigureAwait(false);
            return await this.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Remove All SAclopedia Entry.
        /// </summary>
        /// <returns>Number of rows changed.</returns>
        public async Task<int> RemoveAllSAclopediaEntryAsync()
        {
            var saclopedias = await this.SAclopediaEntryItems.ToListAsync().ConfigureAwait(false);
            this.SAclopediaEntryItems.RemoveRange(saclopedias);
            return await this.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<SAclopediaEntryItem>> GetAllSAclopediaEntryAsync()
        {
            return await this.SAclopediaEntryItems.ToListAsync().ConfigureAwait(false);
        }

        #endregion

        #region Settings

        /// <summary>
        /// Add or update settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Number of rows changed.</returns>
        public async Task<int> AddOrUpdateSettingsAsync(SettingOptions settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var setting = await this.SettingOptionsItems.FirstOrDefaultAsync().ConfigureAwait(false);
            if (setting == null)
            {
                await this.SettingOptionsItems.AddAsync(settings).ConfigureAwait(false);
            }
            else
            {
                this.SettingOptionsItems.Update(settings);
            }

            return await this.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Default Setting Options.
        /// </summary>
        /// <returns>SettingOptions.</returns>
        public async Task<SettingOptions> GetDefaultSettingsAsync()
        {
            var settingOptions = await this.SettingOptionsItems.FirstOrDefaultAsync().ConfigureAwait(false);
            if (settingOptions == null)
            {
                var options = new SettingOptions();
                options.DeviceColorTheme = this.PlatformProperties.GetTheme();
                return options;
            }

            return settingOptions;
        }

        #endregion

        #region Users

        /// <summary>
        /// Test if users exist.
        /// </summary>
        /// <returns>Bool.</returns>
        public async Task<bool> DoesUsersExistAsync()
        {
            return await this.Users.AnyAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public async Task<UserAuth> GetDefaultUserAsync()
        {
            var user = await this.Users.FirstOrDefaultAsync(node => node.IsDefaultUser).ConfigureAwait(false);
            if (user != null)
            {
                user.AuthCookies = CookieManager.LoadCookie(this.PlatformProperties.CookiePath);
            }

            return user;
        }

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public async Task<int> AddOrUpdateUserAsync(UserAuth userAuth)
        {
            if (userAuth == null)
            {
                throw new ArgumentNullException(nameof(userAuth));
            }

            userAuth.IsDefaultUser = true;
            var users = await this.Users.ToListAsync().ConfigureAwait(false);
            foreach (var oldUser in users)
            {
                oldUser.IsDefaultUser = false;
            }

            var user = await this.Users.FirstOrDefaultAsync(node => node.UserAuthId == userAuth.UserAuthId).ConfigureAwait(false);
            if (user == null)
            {
                await this.Users.AddAsync(userAuth).ConfigureAwait(false);
            }
            else
            {
                this.Users.Update(userAuth);
            }

            return await this.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public async Task<int> RemoveUserAsync(UserAuth userAuth)
        {
            if (System.IO.File.Exists(this.PlatformProperties.CookiePath))
            {
                System.IO.File.Delete(this.PlatformProperties.CookiePath);
            }

            this.Users.Remove(userAuth);
            if (await this.Users.AnyAsync(n => n.IsDefaultUser).ConfigureAwait(false) == false)
            {
                var user = await this.Users.FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    user.IsDefaultUser = true;
                }
            }

            return await this.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Remove All Users.
        /// </summary>
        /// <returns>Number of rows changed.</returns>
        public async Task<int> RemoveAllUsersAsync()
        {
            var users = await this.Users.ToListAsync().ConfigureAwait(false);
            this.Users.RemoveRange(users);
            return await this.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        #region Forum Categories

        /// <summary>
        /// Get Forum Categories.
        /// </summary>
        /// <returns>List of Forum Categories.</returns>
        public async Task<List<Forum>> GetForumCategoriesAsync()
        {
            var categories = await this.Forums.Include(y => y.ParentForum).Include(y => y.SubForums).ToListAsync().ConfigureAwait(false);
            foreach (var cat in categories)
            {
                cat.SubForums = cat.SubForums.OrderBy(n => n.SortOrder).ToList();
            }

            return categories;
        }

        /// <summary>
        /// Add or update Forum Categories.
        /// </summary>
        /// <param name="list">List of Forums.</param>
        /// <returns>Updated list of forums.</returns>
        public async Task<List<Forum>> AddOrUpdateForumCategories(List<Forum> list)
        {
            var oldFavorites = await this.Forums.Where(n => n.IsFavorited).Select(n => n.Id).ToListAsync().ConfigureAwait(false);
            var filteredList = list.Where(n => n.Id != 0);
            var forums = filteredList.SelectMany(n => this.Flatten(n));
            var newFavorites = forums.Where(n => oldFavorites.Contains(n.Id));
            foreach (var forum in newFavorites)
            {
                forum.IsFavorited = true;
            }

            this.Forums.RemoveRange(await this.Forums.ToListAsync().ConfigureAwait(false));
            await this.Forums.AddRangeAsync(filteredList).ConfigureAwait(false);
            await this.SaveChangesAsync().ConfigureAwait(false);
            return await this.GetForumCategoriesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Update forum on page.
        /// </summary>
        /// <param name="forum">Forum.</param>
        /// <returns>Updated Forum.</returns>
        public async Task<Forum> UpdateForumAsync(Forum forum)
        {
            var result = this.Forums.Update(forum);
            await this.SaveChangesAsync().ConfigureAwait(false);
            return forum;
        }

        /// <inheritdoc/>
        public async Task<Forum> GetForumAsync(int forumId)
        {
            return await this.Forums.FirstOrDefaultAsync(n => n.Id == forumId).ConfigureAwait(false);
        }

        #endregion

        #region PrivateMessage

        /// <summary>
        /// Add All PMs.
        /// </summary>
        /// <param name="threads">SA Threads.</param>
        /// <returns>SA Database PMs.</returns>
        public async Task<List<AwfulPM>> AddAllPrivateMessages(List<PrivateMessage> threads)
        {
            if (threads == null)
            {
                throw new ArgumentNullException(nameof(threads));
            }

            var oldThreads = await this.PrivateMessages.ToListAsync().ConfigureAwait(false);
            this.PrivateMessages.RemoveRange(oldThreads);
            for (int i = 0; i < threads.Count; i++)
            {
                PrivateMessage thread = (PrivateMessage)threads[i];
                var awfulThread = new AwfulPM(thread);
                awfulThread.SortOrder = i;
                await this.PrivateMessages.AddAsync(awfulThread).ConfigureAwait(false);
            }

            await this.SaveChangesAsync().ConfigureAwait(false);
            return await this.PrivateMessages.OrderBy(n => n.SortOrder).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Remove Private Message.
        /// </summary>
        /// <param name="thread">DB PM.</param>
        /// <returns>List of PMs.</returns>
        public async Task<List<AwfulPM>> RemovePrivateMessage(AwfulPM thread)
        {
            this.PrivateMessages.Remove(thread);
            await this.SaveChangesAsync().ConfigureAwait(false);
            return await this.PrivateMessages.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<AwfulPM>> GetAllPrivateMessages()
        {
            return await this.PrivateMessages.OrderBy(n => n.SortOrder).ToListAsync().ConfigureAwait(false);
        }

        #endregion

        #region Bookmarks

        /// <summary>
        /// Add All Bookmarks.
        /// </summary>
        /// <param name="threads">SA Threads.</param>
        /// <returns>SA Database Threads.</returns>
        public async Task<List<AwfulThread>> AddAllBookmarkThreads(List<Thread> threads)
        {
            if (threads == null)
            {
                throw new ArgumentNullException(nameof(threads));
            }

            var oldThreads = await this.BookmarkThreads.ToListAsync().ConfigureAwait(false);
            this.BookmarkThreads.RemoveRange(oldThreads);
            for (int i = 0; i < threads.Count; i++)
            {
                Thread thread = (Thread)threads[i];
                var awfulThread = new AwfulThread(thread);
                awfulThread.SortOrder = i;
                await this.BookmarkThreads.AddAsync(awfulThread).ConfigureAwait(false);
            }

            await this.SaveChangesAsync().ConfigureAwait(false);
            return await this.BookmarkThreads.OrderBy(n => n.SortOrder).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<AwfulThread>> GetAllBookmarkThreadsAsync()
        {
            return await this.BookmarkThreads.OrderBy(n => n.SortOrder).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Remove Bookmark Thread.
        /// </summary>
        /// <param name="thread">DB Thread.</param>
        /// <returns>List of Threads.</returns>
        public async Task<List<AwfulThread>> RemoveBookmarkThread(AwfulThread thread)
        {
            this.BookmarkThreads.Remove(thread);
            await this.SaveChangesAsync().ConfigureAwait(false);
            return await this.BookmarkThreads.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Enable or disable bookmark notifications for a given thread.
        /// </summary>
        /// <param name="thread">The AwfulThread.</param>
        /// <returns>The AwfulThread with the updated value.</returns>
        public async Task<AwfulThread> EnableDisableBookmarkNotificationsEnable(AwfulThread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            var bookmarkContains = await this.BookmarkThreads.ContainsAsync(thread).ConfigureAwait(false);
            if (!bookmarkContains)
            {
                throw new Exception("Thread is not contained in the bookmark list.");
            }

            if (!thread.IsBookmark)
            {
                throw new Exception("Thread is not listed as a bookmark.");
            }

            thread.EnableBookmarkNotifications = !thread.EnableBookmarkNotifications;
            await this.SaveChangesAsync().ConfigureAwait(false);
            return thread;
        }

        #endregion

        /// <summary>
        /// Run when configuring the database.
        /// </summary>
        /// <param name="optionsBuilder"><see cref="DbContextOptionsBuilder"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={this.PlatformProperties.DatabasePath}");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        /// Run when building the model.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Ignore<Result>();
            modelBuilder.Entity<SAclopediaEntryItem>().HasKey(n => n.Id);
            modelBuilder.Entity<Forum>().Ignore(b => b.Moderators);
            //modelBuilder.Entity<Forum>().Ignore(b => b.SubForums);
            modelBuilder.Entity<Moderator>().HasNoKey();
            modelBuilder.Entity<Forum>().HasMany(y => y.SubForums).WithOne().HasForeignKey(p => p.ParentForumId);
            modelBuilder.Entity<UserAuth>().Ignore(b => b.AuthCookies);
        }

        private IEnumerable<Forum> Flatten(Forum forum)
        {
            yield return forum;
            if (forum.SubForums != null)
            {
                foreach (var child in forum.SubForums)
                {
                    foreach (var descendant in this.Flatten(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
