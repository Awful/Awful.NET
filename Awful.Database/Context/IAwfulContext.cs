// <copyright file="IAwfulContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Entities.Threads;
using Awful.Database.Entities;

namespace Awful.Database.Context
{
    /// <summary>
    /// Awful Database Context.
    /// </summary>
    public interface IAwfulContext
    {
        /// <summary>
        /// Resets the local database.
        /// </summary>
        public void ResetDatabase();

        /// <summary>
        /// Add All SAclopedia Entries.
        /// </summary>
        /// <param name="entries">Entries to be added.</param>
        /// <returns>Number of rows changed.</returns>
        public Task<int> AddAllSAclopediaEntry(List<SAclopediaEntryItem> entries);

        /// <summary>
        /// Remove All SAclopedia Entry.
        /// </summary>
        /// <returns>Number of rows changed.</returns>
        public Task<int> RemoveAllSAclopediaEntryAsync();

        /// <summary>
        /// Get all SAclopedia Entries.
        /// </summary>
        /// <returns>List of SAclopedia Entries.</returns>
        public Task<List<SAclopediaEntryItem>> GetAllSAclopediaEntryAsync();

        /// <summary>
        /// Add or update settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Number of rows changed.</returns>
        public Task<int> AddOrUpdateSettingsAsync(SettingOptions settings);

        /// <summary>
        /// Get Default Setting Options.
        /// </summary>
        /// <returns>SettingOptions.</returns>
        public Task<SettingOptions> GetDefaultSettingsAsync();

        /// <summary>
        /// Test if users exist.
        /// </summary>
        /// <returns>Bool.</returns>
        public Task<bool> DoesUsersExistAsync();

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public Task<UserAuth> GetDefaultUserAsync();

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public Task<int> AddOrUpdateUserAsync(UserAuth userAuth);

        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public Task<int> RemoveUserAsync(UserAuth userAuth);

        /// <summary>
        /// Remove All Users.
        /// </summary>
        /// <returns>Number of rows changed.</returns>
        public Task<int> RemoveAllUsersAsync();

        /// <summary>
        /// Get Forum Categories.
        /// </summary>
        /// <returns>List of Forum Categories.</returns>
        public Task<List<Forum>> GetForumCategoriesAsync();

        /// <summary>
        /// Add or update Forum Categories.
        /// </summary>
        /// <param name="list">List of Forums.</param>
        /// <returns>Updated list of forums.</returns>
        public Task<List<Forum>> AddOrUpdateForumCategories(List<Forum> list);

        /// <summary>
        /// Update forum on page.
        /// </summary>
        /// <param name="forum">Forum.</param>
        /// <returns>Updated Forum.</returns>
        public Task<Forum> UpdateForumAsync(Forum forum);

        /// <summary>
        /// Get forum.
        /// </summary>
        /// <param name="forumId">Forum Id.</param>
        /// <returns>Forum.</returns>
        public Task<Forum> GetForumAsync(int forumId);

        /// <summary>
        /// Add All PMs.
        /// </summary>
        /// <param name="threads">SA PMs.</param>
        /// <returns>SA Database PMs.</returns>
        public Task<List<AwfulPM>> AddAllPrivateMessages(List<PrivateMessage> threads);

        /// <summary>
        /// Remove Private Message.
        /// </summary>
        /// <param name="thread">DB PM.</param>
        /// <returns>List of PMs.</returns>
        public Task<List<AwfulPM>> RemovePrivateMessage(AwfulPM thread);

        /// <summary>
        /// Get All PMs.
        /// </summary>
        /// <returns>SA Database PMs.</returns>
        public Task<List<AwfulPM>> GetAllPrivateMessages();

        /// <summary>
        /// Add All Bookmarks.
        /// </summary>
        /// <param name="threads">SA Threads.</param>
        /// <returns>SA Database Threads.</returns>
        public Task<List<AwfulThread>> AddAllBookmarkThreads(List<Thread> threads);

        /// <summary>
        /// Remove Bookmark Thread.
        /// </summary>
        /// <param name="thread">DB Thread.</param>
        /// <returns>List of Threads.</returns>
        public Task<List<AwfulThread>> RemoveBookmarkThread(AwfulThread thread);

        /// <summary>
        /// Get All Bookmarks.
        /// </summary>
        /// <returns>SA Database Threads.</returns>
        public Task<List<AwfulThread>> GetAllBookmarkThreadsAsync();

        /// <summary>
        /// Enable or disable bookmark notifications for a given thread.
        /// </summary>
        /// <param name="thread">The AwfulThread.</param>
        /// <returns>The AwfulThread with the updated value.</returns>
        public Task<AwfulThread> EnableDisableBookmarkNotificationsEnable(AwfulThread thread);

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose();
    }
}
