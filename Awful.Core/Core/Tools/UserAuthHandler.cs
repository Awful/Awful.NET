using Awful.Managers;
using Awful.Models.Users;
using Awful.Database.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Tools
{
    public class UserAuthHandler
    {
        public static WebManager CreateAuthWebManager(UserAuth user)
        {
            var cookies = CookieManager.LoadCookie(user.CookiePath);
            return new WebManager(cookies);
        }

        public static UserState GetDefaultAuthWebManager()
        {
            var user = UserAuthDatabase.GetDefaultUser();
            if (user == null) return new UserState { IsLoggedIn = false, WebManager = new WebManager() };
            var webManager = CreateAuthWebManager(user);
            return new UserState { IsLoggedIn = true, WebManager = webManager, User = user };
        }
    }

    public class UserState
    {
        public UserAuth User { get; set; }
        public WebManager WebManager { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
