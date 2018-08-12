using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using Awful.Parser.Models.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class UserManager
    {
        private readonly WebClient _webManager;

        public UserManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<User> GetUserFromProfilePageAsync(long userId)
        {
            string url = EndPoints.BaseUrl + string.Format(EndPoints.UserProfile, userId);
            var result = await _webManager.GetDataAsync(url);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return UserHandler.ParseUserFromProfilePage(userId, document);
        }
    }
}
