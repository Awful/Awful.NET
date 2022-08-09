// <copyright file="ThreadReplyManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
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
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var inputs = htmlResult.Document.QuerySelectorAll("input");
                var bookmarkInput = inputs["bookmark"];
                var bookmarks = bookmarkInput != null && bookmarkInput.HasAttribute("checked") ? "yes" : "no";
                var quoteArea = htmlResult.Document.QuerySelector("textarea");
                string quote = string.Empty;
                if (quoteArea != null)
                {
                    // SA does weird stuff with decoding so we need to double decode.
                    quote = System.Net.WebUtility.HtmlDecode(System.Net.WebUtility.HtmlDecode(quoteArea.TextContent));
                }

                return new ThreadReply(quote, editPostId: postId, bookmark: bookmarks) { Result = result };
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
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var inputs = htmlResult.Document.QuerySelectorAll("input");
                var posts = PostHandler.ParsePreviousPosts(htmlResult.Document);
                var formkey = inputs["formkey"];
                var formCookie = inputs["form_cookie"];
                var threadid = inputs["threadid"];
                if (formkey == null || formCookie == null || threadid == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find form infor while getting forum reply cookies page.", new Awful.Core.Entities.SAItem(result));
                }

                var quoteArea = htmlResult.Document.QuerySelector("textarea");
                string quote = string.Empty;
                if (quoteArea != null)
                {
                    // SA does weird stuff with decoding so we need to double decode.
                    quote = System.Net.WebUtility.HtmlDecode(System.Net.WebUtility.HtmlDecode(quoteArea.TextContent));
                }

                var realThreadId = Convert.ToInt32(threadid.TryGetAttribute("value"));

                return new ThreadReply(quote, formkey.TryGetAttribute("value"), formCookie.TryGetAttribute("value"), threadId: realThreadId, posts: posts) { Result = result };
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

            if (forumReplyEntity.FormKey is null || forumReplyEntity.FormCookie is null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity.FormKey));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("postreply"), "action" },
                { new StringContent(forumReplyEntity.ThreadId.ToString()), "threadid" },
                { new StringContent(forumReplyEntity.FormKey), "formkey" },
                { new StringContent(forumReplyEntity.FormCookie), "form_cookie" },
                { new StringContent(forumReplyEntity.Message.HtmlEncode()), "message" },
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
                { new StringContent(forumReplyEntity.Message.HtmlEncode()), "message" },
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
        public async Task<string> CreatePreviewPostAsync(ThreadReply forumReplyEntity, CancellationToken token = default)
        {
            if (forumReplyEntity == null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity));
            }

            if (forumReplyEntity.FormKey is null || forumReplyEntity.FormCookie is null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity.FormKey));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("postreply"), "action" },
                { new StringContent(forumReplyEntity.ThreadId.ToString()), "threadid" },
                { new StringContent(forumReplyEntity.FormKey), "formkey" },
                { new StringContent(forumReplyEntity.FormCookie), "form_cookie" },
                { new StringContent(forumReplyEntity.Message.HtmlEncode()), "message" },
                { new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("2097152"), "MAX_FILE_SIZE" },
                { new StringContent("Submit Reply"), "submit" },
                { new StringContent("Preview Reply"), "preview" },
            };

            // We post to SA the same way we would for a normal reply, but instead of getting a redirect back to the
            // thread, we'll get redirected to back to the reply screen with the preview message on it.
            // From here we can parse that preview and return it to the user.
            var result = await this.webManager.PostFormDataAsync(EndPoints.NewReply, form, false).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var postbody = htmlResult.Document?.QuerySelector(".postbody");
                if (postbody == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find post body while getting forum preview page.", new Awful.Core.Entities.SAItem(result));
                }

                return postbody.InnerHtml;
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
        public async Task<string> CreatePreviewEditPostAsync(ThreadReply forumReplyEntity, CancellationToken token = default)
        {
            if (forumReplyEntity == null)
            {
                throw new ArgumentNullException(nameof(forumReplyEntity));
            }

            using var form = new MultipartFormDataContent
            {
                { new StringContent("updatepost"), "action" },
                { new StringContent(forumReplyEntity.PostId.ToString(CultureInfo.InvariantCulture)), "postid" },
                { new StringContent(forumReplyEntity.Message.HtmlEncode()), "message" },
                { new StringContent(forumReplyEntity.ParseUrl.ToString()), "parseurl" },
                { new StringContent("2097152"), "MAX_FILE_SIZE" },
                { new StringContent("Preview Post"), "preview" },
            };
            var result = await this.webManager.PostFormDataAsync(EndPoints.EditPost, form, false, token).ConfigureAwait(false);
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var postbody = htmlResult.Document.QuerySelector(".postbody");
                if (postbody == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find post body while getting forum preview page.", new Awful.Core.Entities.SAItem(result));
                }

                return postbody.InnerHtml;
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
            if (result is not HtmlResult htmlResult)
            {
                throw new Exceptions.AwfulParserException("Failed to find document.", new Awful.Core.Entities.SAItem(result));
            }

            try
            {
                var quoteString = htmlResult.Document.QuerySelector("textarea");
                if (quoteString == null)
                {
                    throw new Exceptions.AwfulParserException("Failed to find quote string body while getting quote string page.", new Awful.Core.Entities.SAItem(result));
                }

                return System.Net.WebUtility.HtmlDecode(System.Net.WebUtility.HtmlDecode(quoteString.TextContent));
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
