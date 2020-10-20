// <copyright file="AwfulContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Handlers;
using Awful.Core.Tools;
using Awful.Database.Entities;
using Microsoft.EntityFrameworkCore;

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
                user.AuthCookies = CookieManager.LoadCookie(user.CookiePath);
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
            if (System.IO.File.Exists(userAuth.CookiePath))
            {
                System.IO.File.Delete(userAuth.CookiePath);
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

            modelBuilder.Entity<SAclopediaEntryItem>().HasKey(n => n.Id);
            modelBuilder.Entity<UserAuth>().Ignore(b => b.AuthCookies);
        }
    }
}
