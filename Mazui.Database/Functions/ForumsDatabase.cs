using Mazui.Core.Models.Forums;
using Mazui.Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.Database.Functions
{
    public class ForumsDatabase
    {
        public static List<Category> GetMainForumCategories()
        {
            using (var db = new ForumsContext())
            {
                return db.Categories
                    .Include(o => o.ForumList)
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
    }
}
