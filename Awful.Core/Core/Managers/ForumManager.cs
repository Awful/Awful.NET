using Awful.Models.Forums;
using Awful.Models.Web;
using Awful.Parsers;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class ForumManager
    {
        private readonly WebManager _webManager;

        public ForumManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetForumCategoriesAsync(bool parseToJson = true)
        {
            var result = new Result();
            try
            {
                result = await _webManager.GetDataAsync(EndPoints.ForumListPage);
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, "Failed to download forum list", ex.StackTrace, ex.GetType().FullName);
            }
            if (!result.IsSuccess) return result;

            // Got the forum list HTML!
            result.Type = typeof(Category).ToString();

            if (!parseToJson)
                return result;

            try
            {
                result.ResultJson = JsonConvert.SerializeObject(ForumHandler.ParseCategoryList(result.ResultHtml));
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, "Failed to parse forum list", ex.StackTrace, ex.GetType().FullName);
            }

            return result;
        }
    }
}
