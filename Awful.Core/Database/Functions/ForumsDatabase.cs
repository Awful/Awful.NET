using Awful.Models.Forums;
using Awful.Models.Threads;
using Awful.Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Database.Functions
{
    public class ForumsDatabase
    {
        public static List<Category> GetMainForumCategories()
        {
            using (var db = new ForumsContext())
            {
                return db.Categories
                    .Include(o => o.ForumList)
                    .OrderBy(node => node.Order)
                    .ToList();
            }
        }

        public static List<Forum> GetFavoriteForums()
        {
            using (var db = new ForumsContext())
            {
                return db.Forums.Where(node => node.IsBookmarks).ToList();
            }
        }

        public static async Task<int> UpdateForumBookmark(Forum forum)
        {
            using (var db = new ForumsContext())
            {
                forum.IsBookmarks = !forum.IsBookmarks;
                db.Update(forum);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> SaveForumList(List<Category> newCategories)
        {
            using (var db = new ForumsContext())
            {
                var categories = db.Categories.ToList();
                var forums = db.Forums.ToList();
                db.Categories.RemoveRange(categories);
                db.Forums.RemoveRange(forums);
                await db.SaveChangesAsync();
                var count = 1;
                foreach (var item in newCategories)
                {
                    foreach (var forumitem in item.ForumList)
                    {
                        forumitem.Order = count;
                        count++;
                    }
                    await db.Categories.AddAsync(item);
                    await db.Forums.AddRangeAsync(item.ForumList);
                }
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> RemoveBookmarkThreads()
        {
            using (var bds = new ForumsContext())
            {
                var allBooksmarks = await bds.BookmarkedThreads.ToListAsync();
                bds.BookmarkedThreads.RemoveRange(allBooksmarks);
                return await bds.SaveChangesAsync();
            }
        }

        public static async Task<List<Thread>> GetBookmarkedThreadsFromDb()
        {
            using (var bds = new ForumsContext())
            {
                return await bds.BookmarkedThreads.ToListAsync();
            }
        }


        public static async Task<int> RefreshBookmarkedThreads(List<Thread> updatedBookmarkList)
        {
            var notifyThreadIds = new List<int>();
            using (var db = new ForumsContext())
            {
                var allBooksmarks = await db.BookmarkedThreads.ToListAsync();
                var notifyThreads = allBooksmarks.Where(node => node.IsNotified);
                notifyThreadIds = notifyThreads.Select(thread => thread.ThreadId).ToList();
                db.BookmarkedThreads.RemoveRange(allBooksmarks);
                await db.SaveChangesAsync();
            }

            using (var db = new ForumsContext())
            {
                var count = 0;
                foreach (Thread t in updatedBookmarkList)
                {
                    // Force ForumID to GBS, because if we pull from Bookmark list it's 0
                    if (t.ForumId == 0) t.ForumId = 273;
                    if (notifyThreadIds.Contains(t.ThreadId))
                    {
                        t.IsNotified = true;
                    }
                    t.IsBookmark = true;
                    t.OrderNumber = count;
                    count++;
                }

                foreach(var thread in updatedBookmarkList)
                {
                    db.BookmarkedThreads.Add(thread);
                }
                return await db.SaveChangesAsync();
            }
        }

        public static async Task AddRemoveNotification(int id, bool notification)
        {
            using (var bds = new ForumsContext())
            {
                var thread = bds.BookmarkedThreads.FirstOrDefault(node => node.ThreadId == id);
                if (thread == null) return;
                thread.IsNotified = notification;
                bds.BookmarkedThreads.Update(thread);
                await bds.SaveChangesAsync();
            }
        }
    }
}
