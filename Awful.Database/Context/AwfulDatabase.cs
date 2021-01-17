// <copyright file="AwfulDatabase.cs" company="Drastic Actions">
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
using Awful.Core.Handlers;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using LiteDB;

namespace Awful.Database.Context
{
    /// <summary>
    /// Awful Database.
    /// </summary>
    public class AwfulDatabase : IAwfulDatabase, IDisposable
    {
        private const string BookmarkDB = "bookmarks";
        private const string ForumsDB = "forums";
        private const string PrivateMessageDB = "privatemessages";
        private const string SAclopediaDB = "saclopedia";
        private const string OptionsDB = "options";
        private const string UserAuthDB = "userauth";
        private readonly IPlatformProperties properties;
        private readonly LiteDatabase db;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulDatabase"/> class.
        /// </summary>
        /// <param name="properties">Awful Platform Properties.</param>
        public AwfulDatabase(IPlatformProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            this.properties = properties;
            this.db = new LiteDatabase(properties.DatabasePath + ".litedb");
        }

        /// <inheritdoc/>
        public List<AwfulThread> AddAllBookmarkThreads(List<Thread> threads)
        {
            if (threads == null)
            {
                throw new ArgumentNullException(nameof(threads));
            }

            var collection = this.db.GetCollection<AwfulThread>(BookmarkDB);
            collection.DeleteAll();
            var newThreads = new List<AwfulThread>();
            for (int i = 0; i < threads.Count; i++)
            {
                Thread thread = (Thread)threads[i];
                var awfulThread = new AwfulThread(thread);
                awfulThread.SortOrder = i;
                newThreads.Add(awfulThread);
            }

            collection.InsertBulk(newThreads);
            return newThreads.OrderBy(n => n.SortOrder).ToList();
        }

        /// <inheritdoc/>
        public List<AwfulPM> AddAllPrivateMessages(List<PrivateMessage> threads)
        {
            if (threads == null)
            {
                throw new ArgumentNullException(nameof(threads));
            }

            var collection = this.db.GetCollection<PrivateMessage>(PrivateMessageDB);
            collection.DeleteAll();
            var awfulList = new List<AwfulPM>();
            for (int i = 0; i < threads.Count; i++)
            {
                PrivateMessage thread = (PrivateMessage)threads[i];
                var awfulThread = new AwfulPM(thread);
                awfulThread.SortOrder = i;
                awfulList.Add(awfulThread);
            }

            collection.InsertBulk(awfulList);
            return awfulList.OrderBy(n => n.SortOrder).ToList();
        }

        /// <inheritdoc/>
        public int AddAllSAclopediaEntry(List<SAclopediaEntryItem> entries)
        {
            var collection = this.db.GetCollection<SAclopediaEntryItem>(SAclopediaDB);
            return collection.InsertBulk(entries);
        }

        /// <inheritdoc/>
        public List<Forum> AddOrUpdateForumCategories(List<Forum> list)
        {
            var collection = this.db.GetCollection<Forum>(ForumsDB);
            var oldFavorites = collection.Find(n => n.IsFavorited).Select(n => n.Id).ToList();
            var filteredList = list.Where(n => n.Id != 0);
            var forums = filteredList.SelectMany(n => this.Flatten(n));
            var newFavorites = forums.Where(n => oldFavorites.Contains(n.Id));
            foreach (var forum in newFavorites)
            {
                forum.IsFavorited = true;
            }

            collection.DeleteAll();
            collection.InsertBulk(filteredList);
            return GetForumCategories(collection);
        }

        /// <inheritdoc/>
        public bool AddOrUpdateUser(UserAuth userAuth)
        {
            if (userAuth == null)
            {
                throw new ArgumentNullException(nameof(userAuth));
            }

            userAuth.IsDefaultUser = true;
            var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
            var users = collection.FindAll();
            foreach (var oldUser in users)
            {
                oldUser.IsDefaultUser = false;
            }

            return collection.Upsert(userAuth);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public bool DoesUsersExist()
        {
            var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
            return collection.FindAll().Any();
        }

        /// <inheritdoc/>
        public AwfulThread EnableDisableBookmarkNotificationsEnable(AwfulThread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            var bookmarkCollection = this.db.GetCollection<AwfulThread>(BookmarkDB);
            if (!bookmarkCollection.Exists(n => n.Id == thread.Id))
            {
                throw new Exception("Thread is not contained in the bookmark list.");
            }

            if (!thread.IsBookmark)
            {
                throw new Exception("Thread is not listed as a bookmark.");
            }

            thread.EnableBookmarkNotifications = !thread.EnableBookmarkNotifications;
            bookmarkCollection.Update(thread);
            return thread;
        }

        /// <inheritdoc/>
        public List<AwfulThread> GetAllBookmarkThreads()
        {
            var bookmarkCollection = this.db.GetCollection<AwfulThread>(BookmarkDB);
            return bookmarkCollection.FindAll().OrderByDescending(n => n.KilledOn).ToList();
        }

        /// <inheritdoc/>
        public List<AwfulPM> GetAllPrivateMessages()
        {
            var pmCollection = this.db.GetCollection<AwfulPM>(PrivateMessageDB);
            return pmCollection.FindAll().OrderByDescending(n => n.Date).ToList();
        }

        /// <inheritdoc/>
        public List<SAclopediaEntryItem> GetAllSAclopediaEntry()
        {
            var collection = this.db.GetCollection<SAclopediaEntryItem>(SAclopediaDB);
            return collection.FindAll().ToList();
        }

        /// <inheritdoc/>
        public SettingOptions GetAppSettings()
        {
            var collection = this.db.GetCollection<SettingOptions>(OptionsDB);
            var appSettings = collection.FindAll().ToList();
            var appSetting = appSettings.FirstOrDefault();
            if (appSetting != null)
            {
                return appSetting;
            }

            appSetting = new SettingOptions() { UseDarkMode = this.properties.IsDarkTheme };
            return appSetting;
        }

        /// <inheritdoc/>
        public UserAuth GetDefaultUser()
        {
            var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
            var userAuth = collection.FindAll().FirstOrDefault();
            if (userAuth != null)
            {
                userAuth.AuthCookies = CookieManager.LoadCookie(this.properties.CookiePath);
            }

            return userAuth;
        }

        /// <inheritdoc/>
        public Forum GetForum(int forumId)
        {
            var collection = this.db.GetCollection<Forum>(ForumsDB);
            return collection.FindOne(x => x.Id == forumId);
        }

        /// <inheritdoc/>
        public List<Forum> GetForumCategories()
        {
            var collection = this.db.GetCollection<Forum>(ForumsDB);
            return GetForumCategories(collection);
        }

        /// <inheritdoc/>
        public int RemoveAllSAclopediaEntry()
        {
            var collection = this.db.GetCollection<SAclopediaEntryItem>(SAclopediaDB);
            return collection.DeleteAll();
        }

        /// <inheritdoc/>
        public int RemoveAllUsers()
        {
            var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
            return collection.DeleteAll();
        }

        /// <inheritdoc/>
        public List<AwfulThread> RemoveBookmarkThread(AwfulThread thread)
        {
            var collection = this.db.GetCollection<AwfulThread>(BookmarkDB);
            collection.DeleteMany(item => item.ThreadId == thread.ThreadId);
            return this.GetAllBookmarkThreads();
        }

        /// <inheritdoc/>
        public List<AwfulPM> RemovePrivateMessage(AwfulPM thread)
        {
            var collection = this.db.GetCollection<AwfulPM>(PrivateMessageDB);
            collection.DeleteMany(item => item.PrivateMessageId == thread.PrivateMessageId);
            return this.GetAllPrivateMessages();
        }

        /// <inheritdoc/>
        public int RemoveUser(UserAuth userAuth)
        {
            if (System.IO.File.Exists(this.properties.CookiePath))
            {
                System.IO.File.Delete(this.properties.CookiePath);
            }

            var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
            var result = collection.DeleteMany(y => y.UserAuthId == userAuth.UserAuthId);
            var user = collection.FindAll().FirstOrDefault();
            if (user != null)
            {
                user.IsDefaultUser = true;
                collection.Update(user);
            }

            return result;
        }

        /// <inheritdoc/>
        public void ResetDatabase()
        {
            this.db.DropCollection(ForumsDB);
            this.db.DropCollection(BookmarkDB);
            this.db.DropCollection(PrivateMessageDB);
            this.db.DropCollection(SAclopediaDB);
            this.db.DropCollection(OptionsDB);
            this.db.DropCollection(UserAuthDB);
        }

        /// <inheritdoc/>
        public bool SaveAppSettings(SettingOptions appSettings)
        {
            var collection = this.db.GetCollection<SettingOptions>(OptionsDB);
            return collection.Upsert(appSettings);
        }

        /// <inheritdoc/>
        public Forum UpdateForum(Forum forum)
        {
            var collection = this.db.GetCollection<Forum>(ForumsDB);
            return collection.FindOne(n => n.Id == forum.Id);
        }

        /// <summary>
        /// Dispose DB.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.db.Dispose();
            }

            this.isDisposed = true;
        }

        private static List<Forum> GetForumCategories(ILiteCollection<Forum> collection)
        {
            var categories = collection.Include(y => y.ParentForum).Include(y => y.SubForums).FindAll();
            foreach (var cat in categories)
            {
                cat.SubForums = cat.SubForums.OrderBy(n => n.SortOrder).ToList();
            }

            return categories.ToList();
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
