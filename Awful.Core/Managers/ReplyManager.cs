using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Replies;
using Awful.Parser.Models.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Core.Managers
{
    public class ReplyManager
    {
        private readonly WebClient _webManager;

        public ReplyManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<ForumReply> GetReplyCookiesForEditAsync(long postId)
        {
            string url = string.Format(EndPoints.EditBase, postId);
            var result = await _webManager.GetDataAsync(url);
            var document = _webManager.Parser.Parse(result.ResultHtml);
            var inputs = document.QuerySelectorAll("input");
            var forumReplyEntity = new ForumReply();
            var bookmarks = inputs["bookmark"].HasAttribute("checked") ? "yes" : "no";
            string quote = System.Net.WebUtility.HtmlDecode(document.QuerySelector("textarea").TextContent);
            forumReplyEntity.MapEditPostInformation(
                quote,
                postId,
                bookmarks
                );
            return forumReplyEntity;
        }

        public async Task<ForumReply> GetReplyCookiesAsync(long threadId = 0, long postId = 0)
        {
            if (threadId == 0 && postId == 0) return new ForumReply();
            string url;
            url = threadId > 0 ? string.Format(EndPoints.ReplyBase, threadId) : string.Format(EndPoints.QuoteBase, postId);
            var result = await _webManager.GetDataAsync(url);
            var document = _webManager.Parser.Parse(result.ResultHtml);
            var inputs = document.QuerySelectorAll("input");
            var posts = ThreadHandler.ParsePreviousPosts(document);
            var forumReplyEntity = new ForumReply();
            string quote = System.Net.WebUtility.HtmlDecode(document.QuerySelector("textarea").TextContent);
            forumReplyEntity.MapThreadInformation(
                inputs["formkey"].GetAttribute("value"),
                inputs["form_cookie"].GetAttribute("value"),
                quote,
                inputs["threadid"].GetAttribute("value")
                );
            forumReplyEntity.ForumPosts = posts;
            return forumReplyEntity;
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
            catch (Exception)
            {
                return result;
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
            catch (Exception)
            {
                return result;
            }
        }

        public async Task<Post> CreatePreviewPostAsync(ForumReply forumReplyEntity)
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

            var result = await _webManager.PostFormDataAsync(EndPoints.NewReply, form);
            var document = _webManager.Parser.Parse(result.ResultHtml);
            return new Post { PostHtml = document.QuerySelector(".postbody").InnerHtml };
        }

        public async Task<Post> CreatePreviewEditPostAsync(ForumReply forumReplyEntity)
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
            var result = await _webManager.PostFormDataAsync(EndPoints.EditPost, form);
            var document = _webManager.Parser.Parse(result.ResultHtml);
            return new Post { PostHtml = document.QuerySelector(".postbody").InnerHtml };
        }

        public async Task<string> GetQuoteStringAsync(long postId)
        {
            string url = string.Format(EndPoints.QuoteBase, postId);
            var result = await _webManager.GetDataAsync(url);
            var document = _webManager.Parser.Parse(result.ResultHtml);
            return System.Net.WebUtility.HtmlDecode(System.Net.WebUtility.HtmlDecode(document.QuerySelector("textarea").TextContent));
        }
    }
}
