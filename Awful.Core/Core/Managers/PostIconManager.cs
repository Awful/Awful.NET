using Awful.Models.PostIcons;
using Awful.Models.Web;
using Awful.Parsers;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class PostIconManager
    {
        private readonly WebManager _webManager;

        public PostIconManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetPostIconsAsync(bool isPrivateMessage = false, int forumId = 0)
        {
            Result result = new Result();
            var postIconHandler = new PostIconHandler();
            try
            {
                string url = isPrivateMessage ? EndPoints.NewPrivateMessage : string.Format(EndPoints.NewThread, forumId);
                result = await _webManager.GetDataAsync(url);
                var postIconCategoryList = new List<PostIconCategory>();
                PostIconHandler.Parse(postIconCategoryList, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(postIconCategoryList);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }
    }
}
