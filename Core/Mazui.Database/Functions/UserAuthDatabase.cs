using Mazui.Core.Models.Users;
using Mazui.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.Database.Functions
{
    public class UserAuthDatabase
    {
        public static UserAuth GetDefaultUser()
        {
            using (var db = new UserAuthContext())
            {
                return db.Users.FirstOrDefault(node => node.IsDefaultUser);
            }
        }

        public static async Task<int> RemoveUser(UserAuth user)
        {
            using (var db = new UserAuthContext())
            {
                db.Users.Remove(user);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> AddOrUpdateUser(UserAuth userAuth)
        {
            using (var db = new UserAuthContext())
            {
                var user = db.Users.FirstOrDefault(node => node.UserAuthId == userAuth.UserAuthId);
                if (user == null)
                {
                   await db.Users.AddAsync(userAuth);
                } else
                {
                    db.Users.Update(userAuth);
                }
                return await db.SaveChangesAsync();
            }
        }
    }
}
