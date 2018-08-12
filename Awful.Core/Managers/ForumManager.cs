using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class ForumManager
    {
        private readonly WebClient _webManager;

        public ForumManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<List<Category>> GetForumCategoriesViaSelectAsync()
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            var result = await _webManager.GetDataAsync(EndPoints.ForumListPage);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return ForumHandler.ParseCategoryList(document);
        }

        public async Task<Category> GetForumDescriptionsFromCategoryPageAsync(Category category)
        {
            var result = await _webManager.GetDataAsync(string.Format(EndPoints.ForumPage, category.Id));
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return ForumHandler.ParseForumDescriptions(document, category);
        }

        public async Task<Forum> GetForumDescriptionsFromForumPageAsync(Forum forum)
        {
            if (forum.SubForums.Count <= 0)
                return forum;
            var result = await _webManager.GetDataAsync(string.Format(EndPoints.ForumPage, forum.ForumId));
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return ForumHandler.ParseSubForumDescriptions(document, forum);
        }

        public async Task<List<Category>> GetForumCategoriesAsync()
        {
            var result = await _webManager.GetDataAsync(EndPoints.BaseUrl);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return ForumHandler.ParseCategoryList(document);
        }
    }
}
