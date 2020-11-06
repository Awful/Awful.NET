// <copyright file="AwfulContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Entities.Threads;
using Awful.Core.Handlers;
using Awful.Core.Tools;
using Awful.Database.Entities;
using Awful.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Awful.Webview.Entities.Themes;

namespace Awful.Database.Context
{
    /// <summary>
    /// Awful Database Context.
    /// </summary>
    public class AwfulContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulContext"/> class.
        /// </summary>
        /// <param name="platformProperties">Path to the platform properties.</param>
        public AwfulContext(IPlatformProperties platformProperties)
        {
            this.PlatformProperties = platformProperties;
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets or sets the users table.
        /// </summary>
        public DbSet<UserAuth> Users { get; set; }

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

        private IPlatformProperties PlatformProperties { get; set; }

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

            var setting = this.SettingOptionsItems.FirstOrDefault();
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
                return new SettingOptions();
            }

            return settingOptions;
        }

        #endregion

        #region Users

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public UserAuth GetDefaultUser()
        {
            var user = this.Users.FirstOrDefault(node => node.IsDefaultUser);
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
            foreach (var oldUser in this.Users)
            {
                oldUser.IsDefaultUser = false;
            }

            var user = this.Users.FirstOrDefault(node => node.UserAuthId == userAuth.UserAuthId);
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
                this.BookmarkThreads.Add(awfulThread);
            }

            await this.SaveChangesAsync().ConfigureAwait(false);
            return this.BookmarkThreads.OrderBy(n => n.SortOrder).ToList();
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

            if (!this.BookmarkThreads.Contains(thread))
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

            //modelBuilder.Entity<SettingOptions>()
            //    .Property(c => c.DeviceColorTheme)
            //    .HasConversion<int>();
            //modelBuilder.Entity<SettingOptions>()
            //    .Property(c => c.DeviceTheme)
            //    .HasConversion<int>();
            modelBuilder.Entity<SAclopediaEntryItem>().HasKey(n => n.Id);
            modelBuilder.Entity<UserAuth>().Ignore(b => b.AuthCookies);
        }
    }
}
