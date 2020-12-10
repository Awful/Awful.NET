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
using Awful.UI.Interfaces;
using Awful.UI.Tools;
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
        private AwfulAsyncCommand refreshCommand;
        private AwfulAsyncCommand replyToThreadCommand;
        private DefaultOptions defaults;
        private IAwfulNavigation navigation;
        private IAwfulErrorHandler error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public ForumThreadPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.navigation = navigation;
            this.error = error;
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
                    this.error);
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
                    if (this.ThreadPost != null)
                    {
                        await this.navigation.PushModalAsync(new ThreadReplyPage(this.Thread.ThreadId)).ConfigureAwait(false);
                    }
                },
                    () => !this.IsBusy && !this.OnProbation,
                    this.error);
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
                    this.error);
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
                    this.error);
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
                    this.error);
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
                    this.error);
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
        /// Handles post data from Javascript.
        /// </summary>
        /// <param name="data">JSON string from post.</param>
        public void HandleDataFromJavascript(string data)
        {
            var json = JsonConvert.DeserializeObject<WebViewDataInterop>(data);
            switch (json.Type)
            {
                case "showPostMenu":
                    // TODO: Refactor into generic method.
                    // TODO: Place into action? Command?
                    Device.BeginInvokeOnMainThread(async () => {
                        var result = await App.Current.MainPage.DisplayActionSheet("Post Options", "Cancel", null, "Share", "Mark Read", "Quote Post").ConfigureAwait(false);
                        switch (result)
                        {
                            case "Share":
                                await Share.RequestAsync(new ShareTextRequest
                                {
                                    Uri = string.Format(CultureInfo.InvariantCulture, EndPoints.ShowPost, json.Id),
                                    Title = this.Title,
                                }).ConfigureAwait(false);
                                break;
                            case "Mark Read":
                                _ = Task.Run(async () => await this.MarkPostAsUnreadAsync(json.Id).ConfigureAwait(false)).ConfigureAwait(false);
                                break;
                            case "Quote Post":
                                if (!this.OnProbation)
                                {
                                    await this.navigation.PushModalAsync(new ThreadReplyPage(this.Thread.ThreadId, json.Id, false)).ConfigureAwait(false);
                                }

                                break;
                        }
                    });
                    break;
                case "showUserMenu":
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var options = !this.CanPM ? new string[2] { "Profile", "Their Posts" } : new string[3] { "Profile", "PM", "Their Posts" };
                        var result = await App.Current.MainPage.DisplayActionSheet("User Options", "Cancel", null, options).ConfigureAwait(false);
                        switch (result)
                        {
                            case "Profile":
                                await this.navigation.PushDetailPageAsync(new UserProfilePage(json.Id)).ConfigureAwait(false);
                                break;
                            case "PM":
                                await this.navigation.PushModalAsync(new NewPrivateMessagePage(json.Text)).ConfigureAwait(false);
                                break;
                            case "Their Posts":
                                break;
                        }
                    });
                    break;
            }
        }

        private async Task MarkPostAsUnreadAsync(int postId)
        {
            var postIndex = this.ThreadPost.Posts.FirstOrDefault(n => n.PostId == postId);
            if (postIndex != null)
            {
                var result = await this.threadActions.MarkPostAsLastReadAsAsync(this.Thread.ThreadId, postIndex.PostIndex).ConfigureAwait(false);
            }
        }
    }
}
