// <copyright file="AwfulLiteDBContext.cs" company="Drastic Actions">
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
    /// Awful LiteDB Context.
    /// </summary>
    public class AwfulLiteDBContext : IAwfulContext, IDisposable
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
        /// Initializes a new instance of the <see cref="AwfulLiteDBContext"/> class.
        /// </summary>
        /// <param name="properties">Awful Platform Properties.</param>
        public AwfulLiteDBContext(IPlatformProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            this.properties = properties;
            this.db = new LiteDatabase(properties.DatabasePath + ".litedb");
        }

        /// <inheritdoc/>
        public Task<List<AwfulThread>> AddAllBookmarkThreadsAsync(List<Thread> threads)
        {
            if (threads == null)
            {
                throw new ArgumentNullException(nameof(threads));
            }

            var tcs = new TaskCompletionSource<List<AwfulThread>>();
            Task.Run(() =>
            {
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
                tcs.SetResult(newThreads.OrderBy(n => n.SortOrder).ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<AwfulPM>> AddAllPrivateMessagesAsync(List<PrivateMessage> threads)
        {
            var tcs = new TaskCompletionSource<List<AwfulPM>>();
            Task.Run(() =>
            {
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
                tcs.SetResult(awfulList.OrderBy(n => n.SortOrder).ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<int> AddAllSAclopediaEntryAsync(List<SAclopediaEntryItem> entries)
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<SAclopediaEntryItem>(SAclopediaDB);
                tcs.SetResult(collection.InsertBulk(entries));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<Forum>> AddOrUpdateForumCategoriesAsync(List<Forum> list)
        {
            var tcs = new TaskCompletionSource<List<Forum>>();
            Task.Run(() =>
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
                tcs.SetResult(GetForumCategories(collection));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<int> AddOrUpdateSettingsAsync(SettingOptions settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var tcs = new TaskCompletionSource<int>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<SettingOptions>(OptionsDB);
                var result = collection.Upsert(settings);
                if (result)
                {
                    tcs.SetResult(1);
                }
                else
                {
                    tcs.SetResult(0);
                }
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<int> AddOrUpdateUserAsync(UserAuth userAuth)
        {
            if (userAuth == null)
            {
                throw new ArgumentNullException(nameof(userAuth));
            }

            var tcs = new TaskCompletionSource<int>();
            Task.Run(() =>
            {
                userAuth.IsDefaultUser = true;
                var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
                var users = collection.FindAll();
                foreach (var oldUser in users)
                {
                    oldUser.IsDefaultUser = false;
                }

                var result = collection.Upsert(userAuth);
                if (result)
                {
                    tcs.SetResult(1);
                }
                else
                {
                    tcs.SetResult(0);
                }
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<bool> DoesUsersExistAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
                tcs.SetResult(collection.FindAll().Any());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AwfulThread> EnableDisableBookmarkNotificationsEnableAsync(AwfulThread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            var tcs = new TaskCompletionSource<AwfulThread>();
            Task.Run(() =>
            {
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
                tcs.SetResult(thread);
            }).ConfigureAwait(false);
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<AwfulThread>> GetAllBookmarkThreadsAsync()
        {
            var tcs = new TaskCompletionSource<List<AwfulThread>>();
            Task.Run(() =>
            {
                var bookmarkCollection = this.db.GetCollection<AwfulThread>(BookmarkDB);
                tcs.SetResult(bookmarkCollection.FindAll().ToList());
            }).ConfigureAwait(false);
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<AwfulPM>> GetAllPrivateMessagesAsync()
        {
            var tcs = new TaskCompletionSource<List<AwfulPM>>();
            Task.Run(() =>
            {
                var pmCollection = this.db.GetCollection<AwfulPM>(PrivateMessageDB);
                tcs.SetResult(pmCollection.FindAll().ToList());
            }).ConfigureAwait(false);
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<SAclopediaEntryItem>> GetAllSAclopediaEntryAsync()
        {
            var tcs = new TaskCompletionSource<List<SAclopediaEntryItem>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<SAclopediaEntryItem>(SAclopediaDB);
                tcs.SetResult(collection.FindAll().ToList());
            }).ConfigureAwait(false);
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<SettingOptions> GetDefaultSettingsAsync()
        {
            var tcs = new TaskCompletionSource<SettingOptions>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<SettingOptions>(OptionsDB);
                var settings = collection.FindAll().FirstOrDefault();
                if (settings == null)
                {
                    var options = new SettingOptions
                    {
                    };
                    tcs.SetResult(options);
                }
                else
                {
                    tcs.SetResult(settings);
                }
            }).ConfigureAwait(false);
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<UserAuth> GetDefaultUserAsync()
        {
            var tcs = new TaskCompletionSource<UserAuth>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
                var userAuth = collection.FindAll().FirstOrDefault();
                if (userAuth != null)
                {
                    userAuth.AuthCookies = CookieManager.LoadCookie(this.properties.CookiePath);
                }

                tcs.SetResult(userAuth);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<Forum> GetForumAsync(int forumId)
        {
            var tcs = new TaskCompletionSource<Forum>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<Forum>(ForumsDB);
                var forum = collection.FindOne(x => x.Id == forumId);
                tcs.SetResult(forum);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<Forum>> GetForumCategoriesAsync()
        {
            var tcs = new TaskCompletionSource<List<Forum>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<Forum>(ForumsDB);
                tcs.SetResult(GetForumCategories(collection));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<int> RemoveAllSAclopediaEntryAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<SAclopediaEntryItem>(SAclopediaDB);
                tcs.SetResult(collection.DeleteAll());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<int> RemoveAllUsersAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<UserAuth>(UserAuthDB);
                tcs.SetResult(collection.DeleteAll());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<AwfulThread>> RemoveBookmarkThreadAsync(AwfulThread thread)
        {
            var tcs = new TaskCompletionSource<List<AwfulThread>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AwfulThread>(BookmarkDB);
                collection.DeleteMany(item => item.ThreadId == thread.ThreadId);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<AwfulPM>> RemovePrivateMessageAsync(AwfulPM thread)
        {
            var tcs = new TaskCompletionSource<List<AwfulPM>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AwfulPM>(PrivateMessageDB);
                collection.DeleteMany(item => item.PrivateMessageId == thread.PrivateMessageId);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<int> RemoveUserAsync(UserAuth userAuth)
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Run(() =>
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

                tcs.SetResult(result);
            });
            return tcs.Task;
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
        public Task<Forum> UpdateForumAsync(Forum forum)
        {
            var tcs = new TaskCompletionSource<Forum>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<Forum>(ForumsDB);
                tcs.SetResult(collection.FindOne(n => n.Id == forum.Id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
