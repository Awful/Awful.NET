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
using Awful.Mobile.Controls;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
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
        private Command refreshCommand;
        private AwfulThread thread;
        private DefaultOptions defaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumThreadPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
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
        public Command RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new Command(async () =>
                {
                    await this.RefreshThreadAsync().ConfigureAwait(false);
                });
            }
        }

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
        /// Gets the reply to thread command.
        /// </summary>
        public Command ReplyToThreadCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        await PushModalAsync(new ThreadReplyPage(this.thread.ThreadId)).ConfigureAwait(false);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the First Page Command.
        /// </summary>
        public Command FirstPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        await this.LoadTemplate(this.threadPost.ThreadId, 1).ConfigureAwait(false);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the Previous Page Command.
        /// </summary>
        public Command PreviousPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        if (this.threadPost.CurrentPage - 1 >= 1)
                        {
                            await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.CurrentPage - 1).ConfigureAwait(false);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Gets the Next Page Command.
        /// </summary>
        public Command NextPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        if (this.threadPost.CurrentPage + 1 <= this.threadPost.TotalPages)
                        {
                            await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.CurrentPage + 1).ConfigureAwait(false);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Gets the Last Page Command.
        /// </summary>
        public Command LastPageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.ThreadPost != null)
                    {
                        await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.TotalPages).ConfigureAwait(false);
                    }
                });
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
            this.Title = this.ThreadPost.Name;
            if (this.thread != null && this.thread.RepliesSinceLastOpened > 0)
            {
                var test = ((this.ThreadPost.TotalPages - 1) * EndPoints.DefaultNumberPerPage) + this.ThreadPost.Posts.Count;
                this.thread.ReplyCount = test;
                var seenReplies = this.thread.ReplyCount - this.thread.RepliesSinceLastOpened;
                var seenPages = seenReplies / EndPoints.DefaultNumberPerPage;
                if (this.ThreadPost.CurrentPage > seenPages)
                {
                    var readReplies = ((this.ThreadPost.CurrentPage - (seenPages + 1)) * EndPoints.DefaultNumberPerPage) + this.ThreadPost.Posts.Count;
                    var totalReplies = this.thread.RepliesSinceLastOpened - readReplies;
                    if (totalReplies < 0)
                    {
                        totalReplies = 0;
                    }

                    this.thread.RepliesSinceLastOpened = totalReplies;
                    this.thread.OnPropertyChanged("RepliesSinceLastOpened");
                }
            }

            var source = new HtmlWebViewSource();
            source.Html = this.threadPostActions.RenderThreadPostView(this.ThreadPost, defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
            this.IsBusy = false;
        }

        public void LoadThread(AwfulThread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            this.thread = thread;
            this.Title = thread.Name;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.threadPostActions = new ThreadPostActions(this.Client, this.Context, this.handler);
            this.threadActions = new ThreadActions(this.Client, this.Context);
            if (this.thread != null && this.threadPost == null)
            {
                await this.LoadTemplate(this.thread.ThreadId, 0, this.thread.HasSeen).ConfigureAwait(false);
            }
        }

        private async Task MarkPostAsUnreadAsync(int postId)
        {
            var postIndex = this.ThreadPost.Posts.FirstOrDefault(n => n.PostId == postId);
            if (postIndex != null)
            {
                var result = await this.threadActions.MarkPostAsLastReadAsAsync(this.thread.ThreadId, postIndex.PostIndex).ConfigureAwait(false);
            }
        }
    }
}
