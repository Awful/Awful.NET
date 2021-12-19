// <copyright file="ThreadReplyManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Replies;
using Awful.Core.Entities.Web;
using Awful.Core.Handlers;
using Awful.Core.Utilities;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Thread Reply Manager.
    /// </summary>
    public class ThreadReplyManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyManager"/> class.
        /// </summary>
        /// <param name="webManager">Awful Client.</param>
        public ThreadReplyManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Get Reply Cookies for Edit Async.
        /// </summary>
        /// <param name="postId">The Post Id.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>Thread Reply.</returns>
        public async Task<ThreadReply> GetReplyCookiesForEditAsync(int postId, CancellationToken token = default)
        {
            if (postId <= 0)
            {
                throw new FormatException(Awful.Core.Resources.ExceptionMessages.PostIdMissing);
            }

            string url = string.Format(CultureInfo.InvariantCulture, EndPoints.EditBase, postId);
            var result = await this.webManager.GetDataAsync(url, false, token).ConfigureAwait(false);
            try
            {
                var inputs = result.Document.QuerySelectorAll("input");
                var forumReplyEntity = new ThreadReply();
                var bookmarks = inputs["bookmark"].HasAttribute("checked") ? "yes" : "no";
                string quote = System.Net.WebUtility.HtmlDecode(result.Document.QuerySelector("textarea").TextContent);
                forumReplyEntity.MapEditPostInformation(
                    quote,
                    postId,
                    bookmarks);
                return forumReplyEntity;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Get Reply Cookies.
        /// </summary>
        /// <param name="threadId">Thread Id.</param>
        /// <param name="postId">Post Id.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Thread Reply.</returns>
        public async Task<ThreadReply> GetReplyCookiesAsync(int threadId = 0, int postId = 0, CancellationToken token = default)
        {
            if (threadId == 0 && postId == 0)
            {
                throw new FormatException(Awful.Core.Resources.ExceptionMessages.ThreadAndPostIdMissing);
            }

            string url;
            url = threadId > 0 ? string.Format(CultureInfo.InvariantCulture, EndPoints.ReplyBase, threadId) : string.Format(CultureInfo.InvariantCulture, EndPoints.QuoteBase, postId);
            var result = await this.webManager.GetDataAsync(url, false, token).ConfigureAwait(false);
            try
            {
                var inputs = result.Document.QuerySelectorAll("input");
                var posts = PostHandler.ParsePreviousPosts(result.Document);
                var forumReplyEntity = new ThreadReply();
                string quote = System.Net.WebUtility.HtmlDecode(result.Document.QuerySelector("textarea").TextContent);
                forumReplyEntity.MapThreadInformation(
                    inputs["formkey"].TryGetAttribute("value"),
                    inputs["form_cookie"].TryGetAttribute("value"),
                    quote,
                    inputs["threadid"].TryGetAttribute("value"));
                forumReplyEntity.ForumPosts.AddRange(posts);
                forumReplyEntity.Result = result;
                return forumReplyEntity;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Send Post.
        /// </summary>
        /// <param name="forumReplyEntity">Thread Reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendPostAsync(ThreadReply forumReplyEntity, CancellationToken token = default)
        {
            if (forumReplyEntity == null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("postreply"), "action" },
                { new StringContent(forumReplyEntity.ThreadId), "threadid" },
                { new StringContent(forumReplyEntity.FormKey), "formkey" },
                { new StringContent(forumReplyEntity.FormCookie), "form_cookie" },
                { new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message" },
                { new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("2097152"), "MAX_FILE_SIZE" },
                { new StringContent("Submit Reply"), "submit" },
            };
            var result = await this.webManager.PostFormDataAsync(EndPoints.NewReply, form, false, token).ConfigureAwait(false);
            try
            {
                return result;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Send Update Post.
        /// </summary>
        /// <param name="forumReplyEntity">Thread Reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendUpdatePostAsync(ThreadReply forumReplyEntity, CancellationToken token = default)
        {
            if (forumReplyEntity == null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("updatepost"), "action" },
                { new StringContent(forumReplyEntity.PostId.ToString(CultureInfo.InvariantCulture)), "postid" },
                { new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message" },
                { new StringContent(forumReplyEntity.ParseUrl.ToString(CultureInfo.InvariantCulture)), "parseurl" },
                { new StringContent(forumReplyEntity.Bookmark), "bookmark" },
                { new StringContent("2097152"), "MAX_FILE_SIZE" },
                { new StringContent("Save Changes"), "submit" },
            };
            var result = await this.webManager.PostFormDataAsync(EndPoints.EditPost, form, false, token).ConfigureAwait(false);
            try
            {
                return result;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Create Preview Post.
        /// </summary>
        /// <param name="forumReplyEntity">Thread Reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<Post> CreatePreviewPostAsync(ThreadReply forumReplyEntity, CancellationToken token = default)
        {
            if (forumReplyEntity == null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("postreply"), "action" },
                { new StringContent(forumReplyEntity.ThreadId), "threadid" },
                { new StringContent(forumReplyEntity.FormKey), "formkey" },
                { new StringContent(forumReplyEntity.FormCookie), "form_cookie" },
                { new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message" },
                { new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("2097152"), "MAX_FILE_SIZE" },
                { new StringContent("Submit Reply"), "submit" },
                { new StringContent("Preview Reply"), "preview" },
            };

            // We post to SA the same way we would for a normal reply, but instead of getting a redirect back to the
            // thread, we'll get redirected to back to the reply screen with the preview message on it.
            // From here we can parse that preview and return it to the user.
            var result = await this.webManager.PostFormDataAsync(EndPoints.NewReply, form, false).ConfigureAwait(false);
            try
            {
                return new Post { PostHtml = result.Document.QuerySelector(".postbody").InnerHtml, Result = result };
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Create Preview Edit Post.
        /// </summary>
        /// <param name="forumReplyEntity">Thread Reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<Post> CreatePreviewEditPostAsync(ThreadReply forumReplyEntity, CancellationToken token = default)
        {
            if (forumReplyEntity == null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("updatepost"), "action" },
                { new StringContent(forumReplyEntity.PostId.ToString(CultureInfo.InvariantCulture)), "postid" },
                { new StringContent(HtmlHelpers.HtmlEncode(forumReplyEntity.Message)), "message" },
                { new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("2097152"), "MAX_FILE_SIZE" },
                { new StringContent("Preview Post"), "preview" },
            };
            var result = await this.webManager.PostFormDataAsync(EndPoints.EditPost, form, false, token).ConfigureAwait(false);
            try
            {
                return new Post { PostHtml = result.Document.QuerySelector(".postbody").InnerHtml, Result = result };
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Get Quote String.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>HTML string.</returns>
        public async Task<string> GetQuoteStringAsync(int postId, CancellationToken token = default)
        {
            if (postId <= 0)
            {
                throw new FormatException(Awful.Core.Resources.ExceptionMessages.PostIdMissing);
            }

            string url = string.Format(CultureInfo.InvariantCulture, EndPoints.QuoteBase, postId);
            var result = await this.webManager.GetDataAsync(url, false, token).ConfigureAwait(false);
            try
            {
                return System.Net.WebUtility.HtmlDecode(System.Net.WebUtility.HtmlDecode(result.Document.QuerySelector("textarea").TextContent));

            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
