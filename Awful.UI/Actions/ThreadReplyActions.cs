// <copyright file="ThreadReplyActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Replies;
using Awful.Core.Entities.Smilies;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Core.Managers;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Actions.
    /// </summary>
    public class ThreadReplyActions
    {
        private IAwfulContext context;
        private ThreadReplyManager manager;
        private TemplateHandler templates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        /// <param name="handler">Awful Handler.</param>
        public ThreadReplyActions(AwfulClient client, IAwfulContext context, TemplateHandler handler)
        {
            this.manager = new ThreadReplyManager(client);
            this.context = context;
            this.templates = handler;
        }

        /// <summary>
        /// Create base Thread Reply for editing a post.
        /// </summary>
        /// <param name="postId">The post to edit.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>ThreadReply.</returns>
        public async Task<ThreadReply> CreateEditThreadReplyAsync(int postId, CancellationToken token = default)
        {
            return await this.manager.GetReplyCookiesForEditAsync(postId, token).ConfigureAwait(false);
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
            return await this.manager.GetReplyCookiesAsync(threadId, postId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Send Post.
        /// </summary>
        /// <param name="reply">ThreadReply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendPostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await this.manager.SendPostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Send Update Post.
        /// </summary>
        /// <param name="reply">Thread Reply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Result.</returns>
        public async Task<Result> SendUpdatePostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await this.manager.SendUpdatePostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create Preview Edit Post.
        /// </summary>
        /// <param name="reply">ThreadReply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<Post> CreatePreviewEditPostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await this.manager.CreatePreviewEditPostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Create Preview Post.
        /// </summary>
        /// <param name="reply">ThreadReply.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Post.</returns>
        public async Task<Post> CreatePreviewPostAsync(ThreadReply reply, CancellationToken token = default)
        {
            return await this.manager.CreatePreviewPostAsync(reply, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Quote String.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>String.</returns>
        public async Task<string> GetQuoteStringAsync(int postId, CancellationToken token = default)
        {
            return await this.manager.GetQuoteStringAsync(postId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Thread Preview.
        /// </summary>
        /// <param name="post">Post.</param>
        /// <param name="defaultOptions">Default Options.</param>
        /// <returns>String.</returns>
        public string GetThreadPreview(Post post, DefaultOptions defaultOptions)
        {
            var threadPost = new ThreadPost();
            threadPost.Posts.Add(post);
            return this.templates.RenderThreadPostView(threadPost, defaultOptions);
        }
    }
}
