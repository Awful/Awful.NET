using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Models.Web;
using Awful.Parsers;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class PostManager
    {
        private readonly WebManager _webManager;

        public PostManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetUsersPostsInThreadAsync(string location, int userId, int currentPage, bool hasBeenViewed = false, bool goToPageOverride = false, bool autoplayGifs = true)
        {
            return await GetThreadPostsAsync(location += $"&userid={userId}", currentPage, hasBeenViewed, goToPageOverride);
        }

        public async Task<Result> GetThreadPostsAsync(string location, int currentPage, bool hasBeenViewed = false, bool goToPageOverride = false, bool autoplayGifs = true)
        {
            var result = new Result();
            try
            {
                string url = location;
                if (goToPageOverride)
                {
                    url = location + string.Format(EndPoints.PageNumber, currentPage);
                }
                else if (currentPage > 1)
                {
                    url = location + string.Format(EndPoints.PageNumber, currentPage);
                }
                else if (hasBeenViewed)
                {
                    url = location + EndPoints.GotoNewPost;
                }
                else
                {
                    url = location + string.Format(EndPoints.PageNumber, currentPage);
                }
                result = await _webManager.GetDataAsync(url);
                var threadPosts = new ThreadPosts();
                await ThreadPostHandler.GetThread(threadPosts, result.ResultHtml, url, result.AbsoluteUri, autoplayGifs);
                result.ResultJson = JsonConvert.SerializeObject(threadPosts);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }

        }
    }
}
