using Mazui.Core.Managers;
using Mazui.Core.Models.Users;
using Mazui.Database.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.Tools.Authentication
{
    public class UserHandler
    {
        public static async Task<WebManager> CreateAuthWebManager(UserAuth user)
        {
            var cookies = await CookieManager.LoadCookie(user.CookiePath + ".txt");
            return new WebManager(cookies);
        }

        public static async Task<UserState> GetDefaultAuthWebManager()
        {
            var user = UserAuthDatabase.GetDefaultUser();
            if (user == null) return new UserState { IsLoggedIn = false, WebManager = new WebManager() };
            var webManager = await CreateAuthWebManager(user);
            return new UserState { IsLoggedIn = false, WebManager = webManager };
        }
    }

    public class UserState
    {
        public WebManager WebManager { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
