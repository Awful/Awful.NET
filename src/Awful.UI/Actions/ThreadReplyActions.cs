// <copyright file="ThreadReplyActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.Posts;
using Awful.Entities.Replies;
using Awful.Entities.Threads;
using Awful.Entities.Web;
using Awful.Managers;
using Awful.Themes;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Actions.
    /// </summary>
    public class ThreadReplyActions
    {
        private IDatabaseContext context;
        private ThreadReplyManager manager;
        private ITemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="handler">Awful Handler.</param>
        public ThreadReplyActions(AwfulClient client, IDatabaseContext context, ITemplateHandler handler)
        {
            manager = new ThreadReplyManager(client);
            this.context = context;
            templates = handler;
        }

        /// <summary>
        /// Create base Thread Reply for editing a post.
        /// </summary>
        /// <param name="postId">The post to edit.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>ThreadReply.</returns>
        public async Task<ThreadReply> CreateEditThreadReplyAsync(int postId, CancellationToken token = default)
        {
            return await manager.GetReplyCookiesForEditAsync(postId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create base Thread Reply for editing a post.
        /// </summary>
        /// <param name="threadId">The thread.</param>
        /// <param name="postId">The post to quote, use 0 to reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>ThreadReply.</returns>
        public async Task<ThreadReply> CreateThreadReplyAsync(int threadId, int postId = 0, CancellationToken token = default)
        {
            return await manager.GetReplyCookiesAsync(threadId, postId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Send Post.
        /// </summary>
        /// <param name="reply">ThreadReply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendPostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await manager.SendPostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Send Update Post.
        /// </summary>
        /// <param name="reply">Thread Reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendUpdatePostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await manager.SendUpdatePostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create Preview Edit Post.
        /// </summary>
        /// <param name="reply">ThreadReply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<string> CreatePreviewEditPostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await manager.CreatePreviewEditPostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create Preview Post.
        /// </summary>
        /// <param name="reply">ThreadReply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<string> CreatePreviewPostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await manager.CreatePreviewPostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Quote String.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>String.</returns>
        public async Task<string> GetQuoteStringAsync(int postId, CancellationToken token = default)
        {
            return await manager.GetQuoteStringAsync(postId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Thread Preview.
        /// </summary>
        /// <param name="post">Post.</param>
        /// <param name="defaultOptions">Default Options.</param>
        /// <returns>String.</returns>
        public string GetThreadPreview(Post post, DefaultOptions defaultOptions)
        {
            var threadPost = new ThreadPost(post);
            return templates.RenderThreadPostView(threadPost, defaultOptions);
        }
    }
}
