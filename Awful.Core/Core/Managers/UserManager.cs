using Awful.Models.Users;
using Awful.Models.Web;
using Awful.Parsers;
using Awful.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class UserManager
    {
        private readonly WebManager _webManager;

        public UserManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetUserFromProfilePageAsync(long userId, bool parseToJson = true)
        {
            string url = EndPoints.BaseUrl + string.Format(EndPoints.UserProfile, userId);
            var result = await _webManager.GetDataAsync(url);
            if (!result.IsSuccess || !parseToJson)
                return ErrorHandler.CreateErrorObject(result, "Failed to get user", "");

            try
            {
                var User = new User();
                UserHandler.ParseFromUserProfile(User, result.ResultHtml);
                User.Id = userId;
                result.ResultJson = JsonConvert.SerializeObject(User);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }
    }
}
