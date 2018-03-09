using Awful.Models.Posts;
using Awful.Models.Replies;
using Awful.Models.Web;
using Awful.Tools;
using Awful.Parsers;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class ReplyManager
    {
        private readonly WebManager _webManager;

        public ReplyManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> SendPostAsync(ForumReply forumReplyEntity)
        {
            var result = new Result();
            try
            {
                var form = new MultipartFormDataContent
            {
                {new StringContent("postreply"), "action"},
                {new StringContent(forumReplyEntity.ThreadId), "threadid"},
                {new StringContent(forumReplyEntity.FormKey), "formkey"},
                {new StringContent(forumReplyEntity.FormCookie), "form_cookie"},
                {new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message"},
                {new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl"},
                {new StringContent("2097152"), "MAX_FILE_SIZE"},
                {new StringContent("Submit Reply"), "submit"}
            };
                result = await _webManager.PostFormDataAsync(EndPoints.NewReply, form);

                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> SendUpdatePostAsync(ForumReply forumReplyEntity)
        {
            var result = new Result();
            try
            {
                var form = new MultipartFormDataContent
            {
                {new StringContent("updatepost"), "action"},
                {new StringContent(forumReplyEntity.PostId.ToString()), "postid"},
                {new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message"},
                {new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl"},
                {new StringContent(forumReplyEntity.Bookmark), "bookmark"},
                {new StringContent("2097152"), "MAX_FILE_SIZE"},
                {new StringContent("Save Changes"), "submit"}
            };
                result = await _webManager.PostFormDataAsync(EndPoints.EditPost, form);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> CreatePreviewEditPost(ForumReply forumReplyEntity)
        {
            var result = new Result();
            try
            {
                var form = new MultipartFormDataContent
            {
                {new StringContent("updatepost"), "action"},
                {new StringContent(forumReplyEntity.PostId.ToString()), "postid"},
                {new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message"},
                {new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl"},
                {new StringContent("2097152"), "MAX_FILE_SIZE"},
                {new StringContent("Preview Post"), "preview"}
            };
                result = await _webManager.PostFormDataAsync(EndPoints.EditPost, form);
                var post = new Post();
                ReplyHandler.ParsePostPreview(post, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(post);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> CreatePreviewPostAsync(ForumReply forumReplyEntity)
        {
            var result = new Result();
            try
            {
                var form = new MultipartFormDataContent
            {
                {new StringContent("postreply"), "action"},
                {new StringContent(forumReplyEntity.ThreadId), "threadid"},
                {new StringContent(forumReplyEntity.FormKey), "formkey"},
                {new StringContent(forumReplyEntity.FormCookie), "form_cookie"},
                {new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message"},
                {new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl"},
                {new StringContent("2097152"), "MAX_FILE_SIZE"},
                {new StringContent("Submit Reply"), "submit"},
                {new StringContent("Preview Reply"), "preview"}
            };
                // We post to SA the same way we would for a normal reply, but instead of getting a redirect back to the
                // thread, we'll get redirected to back to the reply screen with the preview message on it.
                // From here we can parse that preview and return it to the user.

                result = await _webManager.PostFormDataAsync(EndPoints.NewReply, form);
                var post = new Post();
                ReplyHandler.ParsePostPreview(post, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(post);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> GetReplyCookiesForEdit(long postId)
        {
            var result = new Result();
            try
            {
                string url = string.Format(EndPoints.EditBase, postId);
                result = await _webManager.GetDataAsync(url);
                var forumReplyEntity = new ForumReply();
                ReplyHandler.ParsePostReply(forumReplyEntity, postId, result.ResultHtml);

                var threadManager = new ThreadManager(_webManager);
                //Get previous posts from quote page.
                string url2 = string.Format(EndPoints.QuoteBase, postId);
                var result2 = await _webManager.GetDataAsync(url2);
                var forumThreadPosts = new List<Post>();
                ReplyHandler.ParsePreviousPosts(forumThreadPosts, result.ResultHtml);

                forumReplyEntity.ForumPosts = forumThreadPosts;
                result.ResultJson = JsonConvert.SerializeObject(forumReplyEntity);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result> GetReplyCookies(long threadId = 0, long postId = 0)
        {
            var result = new Result();
            try
            {
                string url;
                url = threadId > 0 ? string.Format(EndPoints.ReplyBase, threadId) : string.Format(EndPoints.QuoteBase, postId);
                result = await _webManager.GetDataAsync(url);
                var forumReplyEntity = new ForumReply();
                ReplyHandler.ParseReplyCookies(forumReplyEntity, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(forumReplyEntity);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
            }
        }
    }
}
