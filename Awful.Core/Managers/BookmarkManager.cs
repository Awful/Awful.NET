using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class BookmarkManager
    {
        private readonly WebClient _webManager;

        public BookmarkManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<List<Thread>> GetAllBookmarks()
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            var threadList = new List<Thread>();
            var page = 1;
            while (true)
            {
                var threads = await GetBookmarkList(page);
                if (!threads.Any())
                    break;
                threadList.AddRange(threads);
                page++;
            }
            return threadList;
        }

        public async Task<List<Thread>> GetBookmarkList(int page)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            string url = EndPoints.BookmarksUrl;
            if (page >= 0)
                url = EndPoints.BookmarksUrl + string.Format(EndPoints.PageNumber, page);
            var result = await _webManager.GetDataAsync(url);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return ThreadHandler.ParseForumThreadList(document, 0);
        }

        public async Task<Result> AddBookmarkAsync(long threadId)
        {
            try
            {
                var dic = new Dictionary<string, string>
                {
                    ["json"] = "1",
                    ["action"] = "add",
                    ["threadid"] = threadId.ToString()
                };
                var header = new FormUrlEncodedContent(dic);
                return await _webManager.PostDataAsync(EndPoints.Bookmark, header);
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(new Result(false), ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> RemoveBookmarkAsync(long threadId)
        {
            var dic = new Dictionary<string, string>
            {
                ["json"] = "1",
                ["action"] = "remove",
                ["threadid"] = threadId.ToString()
            };
            var header = new FormUrlEncodedContent(dic);
            return await _webManager.PostDataAsync(EndPoints.Bookmark, header);
        }
    }
}
