// <copyright file="IAwfulDatabase.cs" company="Drastic Actions">
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
    public interface IAwfulDatabase
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
        public int AddAllSAclopediaEntry(List<SAclopediaEntryItem> entries);

        /// <summary>
        /// Remove All SAclopedia Entry.
        /// </summary>
        /// <returns>Number of rows changed.</returns>
        public int RemoveAllSAclopediaEntry();

        /// <summary>
        /// Get all SAclopedia Entries.
        /// </summary>
        /// <returns>List of SAclopedia Entries.</returns>
        public List<SAclopediaEntryItem> GetAllSAclopediaEntry();

        /// <summary>
        /// Gets app settings.
        /// </summary>
        /// <returns>App Settings.</returns>
        SettingOptions GetAppSettings();

        /// <summary>
        /// Save App Settings.
        /// </summary>
        /// <param name="appSettings">App Settings.</param>
        /// <returns>Bool if saved.</returns>
        bool SaveAppSettings(SettingOptions appSettings);

        /// <summary>
        /// Test if users exist.
        /// </summary>
        /// <returns>Bool.</returns>
        public bool DoesUsersExist();

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public UserAuth GetDefaultUser();

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>If user added or changed.</returns>
        public bool AddOrUpdateUser(UserAuth userAuth);

        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public int RemoveUser(UserAuth userAuth);

        /// <summary>
        /// Remove All Users.
        /// </summary>
        /// <returns>Number of rows changed.</returns>
        public int RemoveAllUsers();

        /// <summary>
        /// Get Forum Categories.
        /// </summary>
        /// <returns>List of Forum Categories.</returns>
        public List<Forum> GetForumCategories();

        /// <summary>
        /// Add or update Forum Categories.
        /// </summary>
        /// <param name="list">List of Forums.</param>
        /// <returns>Updated list of forums.</returns>
        public List<Forum> AddOrUpdateForumCategories(List<Forum> list);

        /// <summary>
        /// Update forum on page.
        /// </summary>
        /// <param name="forum">Forum.</param>
        /// <returns>Updated Forum.</returns>
        public Forum UpdateForum(Forum forum);

        /// <summary>
        /// Get forum.
        /// </summary>
        /// <param name="forumId">Forum Id.</param>
        /// <returns>Forum.</returns>
        public Forum GetForum(int forumId);

        /// <summary>
        /// Add All PMs.
        /// </summary>
        /// <param name="threads">SA PMs.</param>
        /// <returns>SA Database PMs.</returns>
        public List<AwfulPM> AddAllPrivateMessages(List<PrivateMessage> threads);

        /// <summary>
        /// Remove Private Message.
        /// </summary>
        /// <param name="thread">DB PM.</param>
        /// <returns>List of PMs.</returns>
        public List<AwfulPM> RemovePrivateMessage(AwfulPM thread);

        /// <summary>
        /// Get All PMs.
        /// </summary>
        /// <returns>SA Database PMs.</returns>
        public List<AwfulPM> GetAllPrivateMessages();

        /// <summary>
        /// Add All Bookmarks.
        /// </summary>
        /// <param name="threads">SA Threads.</param>
        /// <returns>SA Database Threads.</returns>
        public List<AwfulThread> AddAllBookmarkThreads(List<Thread> threads);

        /// <summary>
        /// Remove Bookmark Thread.
        /// </summary>
        /// <param name="thread">DB Thread.</param>
        /// <returns>List of Threads.</returns>
        public List<AwfulThread> RemoveBookmarkThread(AwfulThread thread);

        /// <summary>
        /// Get All Bookmarks.
        /// </summary>
        /// <returns>SA Database Threads.</returns>
        public List<AwfulThread> GetAllBookmarkThreads();

        /// <summary>
        /// Enable or disable bookmark notifications for a given thread.
        /// </summary>
        /// <param name="thread">The AwfulThread.</param>
        /// <returns>The AwfulThread with the updated value.</returns>
        public AwfulThread EnableDisableBookmarkNotificationsEnable(AwfulThread thread);

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose();
    }
}
