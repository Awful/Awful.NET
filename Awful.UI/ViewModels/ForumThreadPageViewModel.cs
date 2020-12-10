// <copyright file="ForumThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Forum Thread Page View Model.
    /// </summary>
    public class ForumThreadPageViewModel : AwfulWebviewViewModel
    {
        private TemplateHandler handler;
        private ThreadPostActions threadPostActions;
        private ThreadActions threadActions;
        private ThreadPost threadPost;
        private AwfulAsyncCommand refreshCommand;
        private AwfulAsyncCommand replyToThreadCommand;
        private DefaultOptions defaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public ForumThreadPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, AwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the current state of the view.
        /// </summary>
        public ThreadPost ThreadPost
        {
            get { return this.threadPost; }
            set { this.SetProperty(ref this.threadPost, value); }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public AwfulAsyncCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new AwfulAsyncCommand(
                    async () =>
                {
                    await this.RefreshThreadAsync().ConfigureAwait(false);
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the reply to thread command.
        /// </summary>
        public AwfulAsyncCommand ReplyToThreadCommand
        {
            get
            {
                return this.replyToThreadCommand ??= new AwfulAsyncCommand(
                    async () =>
                {
                    await this.NavigateToThreadReplyPageAsync().ConfigureAwait(false);
                },
                    () => !this.IsBusy && !this.OnProbation,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the First Page Command.
        /// </summary>
        public AwfulAsyncCommand FirstPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        await this.LoadTemplate(this.threadPost.ThreadId, 1).ConfigureAwait(false);
                    }
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Previous Page Command.
        /// </summary>
        public AwfulAsyncCommand PreviousPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        if (this.threadPost.CurrentPage - 1 >= 1)
                        {
                            await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.CurrentPage - 1).ConfigureAwait(false);
                        }
                    }
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Next Page Command.
        /// </summary>
        public AwfulAsyncCommand NextPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        if (this.threadPost.CurrentPage + 1 <= this.threadPost.TotalPages)
                        {
                            await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.CurrentPage + 1).ConfigureAwait(false);
                        }
                    }
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Last Page Command.
        /// </summary>
        public AwfulAsyncCommand LastPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.TotalPages).ConfigureAwait(false);
                    }
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Refreshes the thread.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task RefreshThreadAsync()
        {
            if (this.ThreadPost != null)
            {
                this.IsRefreshing = true;
                await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.CurrentPage).ConfigureAwait(false);
                this.IsRefreshing = false;
            }
        }

        /// <summary>
        /// Loads Thread Template into webview.
        /// </summary>
        /// <param name="threadId">Thread Id to load.</param>
        /// <param name="pageNumber">Page Number to load.</param>
        /// <param name="gotoNewestPost">Go to newest post on page. Ignores page number.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task LoadTemplate(int threadId, int pageNumber, bool gotoNewestPost = false)
        {
            this.IsBusy = true;
            this.defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            this.ThreadPost = await this.threadPostActions.GetThreadPostsAsync(threadId, pageNumber, gotoNewestPost).ConfigureAwait(false);
            this.OnProbation = this.ThreadPost.Result.OnProbation;
            this.OnProbationText = this.ThreadPost.Result.OnProbationText;
            this.Title = this.ThreadPost.Name;
            if (this.Thread != null && this.Thread.RepliesSinceLastOpened > 0)
            {
                var test = ((this.ThreadPost.TotalPages - 1) * EndPoints.DefaultNumberPerPage) + this.ThreadPost.Posts.Count;
                this.Thread.ReplyCount = test;
                var seenReplies = this.Thread.ReplyCount - this.Thread.RepliesSinceLastOpened;
                var seenPages = seenReplies / EndPoints.DefaultNumberPerPage;
                if (this.ThreadPost.CurrentPage > seenPages)
                {
                    var readReplies = ((this.ThreadPost.CurrentPage - (seenPages + 1)) * EndPoints.DefaultNumberPerPage) + this.ThreadPost.Posts.Count;
                    var totalReplies = this.Thread.RepliesSinceLastOpened - readReplies;
                    if (totalReplies < 0)
                    {
                        totalReplies = 0;
                    }

                    this.Thread.RepliesSinceLastOpened = totalReplies;
                    this.Thread.OnPropertyChanged("RepliesSinceLastOpened");
                }
            }

            this.WebView.SetSource(this.threadPostActions.RenderThreadPostView(this.ThreadPost, this.defaults));
            this.IsBusy = false;
        }

        /// <summary>
        /// Loads the thread into the VM.
        /// </summary>
        /// <param name="thread"><see cref="AwfulThread"/>.</param>
        public void LoadThread(AwfulThread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            this.Thread = thread;
            this.Title = thread.Name;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.threadPostActions = new ThreadPostActions(this.Client, this.Context, this.handler);
            this.threadActions = new ThreadActions(this.Client, this.Context);
            if (this.Thread != null && this.threadPost == null)
            {
                await this.LoadTemplate(this.Thread.ThreadId, 0, this.Thread.HasSeen).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.ReplyToThreadCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Navigate to Thread Reply Page.
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual Task NavigateToThreadReplyPageAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles post data from Javascript.
        /// </summary>
        /// <param name="data">JSON string from post.</param>
        protected virtual void HandleDataFromJavascript(string data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark posts as Unread.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <returns>Task.</returns>
        protected async Task MarkPostAsUnreadAsync(int postId)
        {
            var postIndex = this.ThreadPost.Posts.FirstOrDefault(n => n.PostId == postId);
            if (postIndex != null)
            {
                var result = await this.threadActions.MarkPostAsLastReadAsAsync(this.Thread.ThreadId, postIndex.PostIndex).ConfigureAwait(false);
            }
        }
    }
}
