using Awful.Database.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Parser.Core;
using Awful.Database.Context;
using Awful.Parser.Handlers;

namespace Awful.Tools
{
    public class UserAuthHandler
    {
        public static WebClient CreateAuthWebManager(UserAuth user)
        {
            var cookies = CookieManager.LoadCookie(user.CookiePath);
            return new WebClient(cookies);
        }

        public static UserState GetDefaultAuthWebManager()
        {
            var user = UserAuthDatabase.GetDefaultUser();
            if (user == null) return new UserState { IsLoggedIn = false, WebManager = new WebClient() };
            var webManager = CreateAuthWebManager(user);
            return new UserState { IsLoggedIn = true, WebManager = webManager, User = user };
        }
    }

    public class UserState
    {
        public UserAuth User { get; set; }
        public WebClient WebManager { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
