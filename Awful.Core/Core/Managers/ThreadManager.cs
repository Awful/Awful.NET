using Awful.Models.Forums;
using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Models.Web;
using Awful.Parsers;
using Awful.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class ThreadManager
    {
        private readonly WebManager _webManager;

        public ThreadManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> MarkThreadUnreadAsync(long threadId)
        {
            var result = new Result();
            try
            {
                var dic = new Dictionary<string, string>
                {
                    ["json"] = "1",
                    ["action"] = "resetseen",
                    ["threadid"] = threadId.ToString()
                };
                var header = new FormUrlEncodedContent(dic);
                result = await _webManager.PostDataAsync(EndPoints.ResetBase, header);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> GetBookmarksAsync(int page)
        {
            var result = new Result();
            try
            {
                var forumThreadList = new List<Thread>();
                var forum = new Forum()
                {
                    Name = "Bookmarks",
                    IsSubforum = false,
                    Location = EndPoints.UserCp
                };
                String url = EndPoints.BookmarksUrl;
                if (page >= 0)
                {
                    url = EndPoints.BookmarksUrl + string.Format(EndPoints.PageNumber, page);
                }

                result = (await _webManager.GetDataAsync(url));
                ThreadHandler.ParseThreadList(forumThreadList, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(forumThreadList);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
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

        public async Task<Result> GetThreadCookiesAsync(int forumId)
        {
            var result = new Result();
            try
            {
                string url = string.Format(EndPoints.NewThread, forumId);
                result = await _webManager.GetDataAsync(url);
                var newForumEntity = new NewThread();
                ThreadHandler.ParseNewThread(newForumEntity, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(newForumEntity);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> CreateNewThreadAsync(NewThread newThreadEntity)
        {
            try
            {
                var form = new MultipartFormDataContent
            {
                {new StringContent("postthread"), "action"},
                {new StringContent(newThreadEntity.ForumId.ToString(CultureInfo.InvariantCulture)), "forumid"},
                {new StringContent(newThreadEntity.FormKey), "formkey"},
                {new StringContent(newThreadEntity.FormCookie), "form_cookie"},
                {new StringContent(newThreadEntity.PostIcon.Id.ToString(CultureInfo.InvariantCulture)), "iconid"},
                {new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Subject)), "subject"},
                {new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Content)), "message"},
                {new StringContent(newThreadEntity.ParseUrl.ToString()), "parseurl"},
                {new StringContent("Submit Reply"), "submit"}
            };
                return await _webManager.PostFormDataAsync(EndPoints.NewThreadBase, form);
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(new Result(false), ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> CreateNewThreadPreview(NewThread newThreadEntity)
        {
            var result = new Result();

            // We post to SA the same way we would for a normal reply, but instead of getting a redirect back to the
            // thread, we'll get redirected to back to the reply screen with the preview message on it.
            // From here we can parse that preview and return it to the user.
            try
            {
                var form = new MultipartFormDataContent
            {
                {new StringContent("postthread"), "action"},
                {new StringContent(newThreadEntity.ForumId.ToString(CultureInfo.InvariantCulture)), "forumid"},
                {new StringContent(newThreadEntity.FormKey), "formkey"},
                {new StringContent(newThreadEntity.FormCookie), "form_cookie"},
                {new StringContent(newThreadEntity.PostIcon.Id.ToString(CultureInfo.InvariantCulture)), "iconid"},
                {new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Subject)), "subject"},
                {new StringContent(HtmlHelpers.HtmlEncode(newThreadEntity.Content)), "message"},
                {new StringContent(newThreadEntity.ParseUrl.ToString()), "parseurl"},
                {new StringContent("Submit Post"), "submit"},
                {new StringContent("Preview Post"), "preview"}
            };
                result = await _webManager.PostFormDataAsync(EndPoints.NewThreadBase, form);
                var post = new Post();
                ThreadHandler.ParseNewThreadPreview(post, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(post);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> GetThreadInfoAsync(long threadId, bool getLastPagePosts = false)
        {
            var result = new Result();
            try
            {
                var url = string.Format(EndPoints.ThreadPageLast, threadId);
                result = await _webManager.GetDataAsync(url);
                var thread = new Thread();
                thread.Posts = new List<Post>();
                ThreadPostHandler.GetThread(thread, result.ResultHtml, url, result.AbsoluteUri);
                result.ResultJson = JsonConvert.SerializeObject(thread);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> GetForumThreadsAsync(Forum forum, int page, bool parseToJson = true)
        {
            return await GetForumThreadsAsync(forum.Location, forum.ForumId, page, parseToJson);
        }
        public async Task<Result> GetForumThreadsAsync(string forumLocation, int forumId, int page, bool parseToJson = true)
        {
            var result = new Result();
            var threadHandler = new ThreadHandler();
            try
            {
                string url = forumLocation + string.Format(EndPoints.PageNumber, page);
                result = await _webManager.GetDataAsync(url);
                if (!result.IsSuccess)
                {
                    ErrorHandler.CreateErrorObject(result, "Failed to get threads", string.Empty);
                    return result;
                }

                if (!parseToJson)
                {
                    return result;
                }

                var forumThreadList = new List<Thread>();
                ThreadHandler.ParseForumThreads(forumThreadList, result.ResultHtml, forumId);
                result.ResultJson = JsonConvert.SerializeObject(forumThreadList);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> MarkPostAsLastReadAsAsync(long threadId, int index)
        {
            try
            {
                return await _webManager.GetDataAsync(string.Format(EndPoints.LastRead, index, threadId));
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(new Result(false), ex.Message, ex.StackTrace);
            }
        }
    }
}
