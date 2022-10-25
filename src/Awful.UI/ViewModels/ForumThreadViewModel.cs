// <copyright file="ForumThreadViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.Threads;
using Awful.UI.Actions;
using Awful.UI.Controls;
using Awful.UI.Entities;
using Awful.UI.Tools;
using System.Text.Json;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Forum Thread Page View Model.
    /// </summary>
    public class ForumThreadViewModel : AwfulWebviewViewModel
    {
        protected AsyncCommand? replyToThreadCommand;
        private ThreadPostActions threadPostActions;
        private ThreadActions threadActions;
        private ThreadPost threadPost;
        private AsyncCommand? refreshCommand;
        private AsyncCommand? scrollToBottomCommand;
        private AsyncCommand? scrollToTopCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public ForumThreadViewModel(AwfulThread thread, ThreadPost threadPost, IAwfulWebview webview, Action<string>? callback, IServiceProvider services)
            : base(webview, callback, services)
        {
            threadPostActions = new ThreadPostActions(Client, Context, Templates);
            threadActions = new ThreadActions(Client, Context);
            this.threadPost = threadPost;
            Thread = thread;
        }

        /// <summary>
        /// Gets the reply to thread command.
        /// </summary>
        public AsyncCommand ReplyToThreadCommand
        {
            get
            {
                return replyToThreadCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (ThreadPost != null)
                        {
                            // await this.Navigation.PushModalAsync(new ThreadReplyPage(this.Thread.ThreadId)).ConfigureAwait(false);
                        }
                    },
                    () => !IsBusy && !OnProbation,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the scroll to top command.
        /// </summary>
        public AsyncCommand ScrollToTopCommand
        {
            get
            {
                return scrollToTopCommand ??= new AsyncCommand(
                    async () =>
                    {
                        await WebView.EvaluateJavaScriptAsync("scrollToTop()");
                    },
                    () => !IsBusy,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the scroll to top command.
        /// </summary>
        public AsyncCommand ScrollToBottomCommand
        {
            get
            {
                return scrollToBottomCommand ??= new AsyncCommand(
                    async () =>
                    {
                        await WebView.EvaluateJavaScriptAsync("scrollToBottom()");
                    },
                    () => !IsBusy,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets or sets the current state of the view.
        /// </summary>
        public ThreadPost ThreadPost
        {
            get { return threadPost; }
            set { this.SetProperty(ref threadPost, value); }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public AsyncCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??= new AsyncCommand(
                    async () =>
                    {
                        await RefreshThreadAsync().ConfigureAwait(false);
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the First Page Command.
        /// </summary>
        public AsyncCommand FirstPageCommand
        {
            get
            {
                return new AsyncCommand(
                    async () =>
                    {
                        if (ThreadPost != null)
                        {
                            await this.LoadTemplate(threadPost.ThreadId, 1).ConfigureAwait(false);
                        }
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Previous Page Command.
        /// </summary>
        public AsyncCommand PreviousPageCommand
        {
            get
            {
                return new AsyncCommand(
                    async () =>
                    {
                        if (ThreadPost != null)
                        {
                            if (threadPost.CurrentPage - 1 >= 1)
                            {
                                await this.LoadTemplate(threadPost.ThreadId, threadPost.CurrentPage - 1).ConfigureAwait(false);
                            }
                        }
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Next Page Command.
        /// </summary>
        public AsyncCommand NextPageCommand
        {
            get
            {
                return new AsyncCommand(
                    async () =>
                    {
                        if (ThreadPost != null)
                        {
                            if (threadPost.CurrentPage + 1 <= threadPost.TotalPages)
                            {
                                await this.LoadTemplate(threadPost.ThreadId, threadPost.CurrentPage + 1).ConfigureAwait(false);
                            }
                        }
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Last Page Command.
        /// </summary>
        public AsyncCommand LastPageCommand
        {
            get
            {
                return new AsyncCommand(
                    async () =>
                    {
                        if (ThreadPost != null)
                        {
                            await this.LoadTemplate(threadPost.ThreadId, threadPost.TotalPages).ConfigureAwait(false);
                        }
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Handles Data From javascript.
        /// </summary>
        /// <param name="data">json string.</param>
        public void HandleDataFromJavascript(string data)
        {
            var json = JsonSerializer.Deserialize<WebViewDataInterop>(data);
            if (json is null)
            {
                return;
            }

            switch (json.Type)
            {
                case "showPostMenu":
                    // TODO: Refactor into generic method.
                    // TODO: Place into action? Command?
                    Dispatcher.Dispatch(async () =>
                    {
                        //var result = await App.Current.MainPage.DisplayActionSheet("Post Options", "Cancel", null, "Share", "Mark Read", "Quote Post").ConfigureAwait(false);
                        //switch (result)
                        //{
                        //    case "Share":
                        //        await Share.RequestAsync(new ShareTextRequest
                        //        {
                        //            Uri = string.Format(CultureInfo.InvariantCulture, EndPoints.ShowPost, json.Id),
                        //            Title = this.Title,
                        //        }).ConfigureAwait(false);
                        //        break;
                        //    case "Mark Read":
                        //        _ = Task.Run(async () => await this.MarkPostAsUnreadAsync(json.Id).ConfigureAwait(false)).ConfigureAwait(false);
                        //        break;
                        //    case "Quote Post":
                        //        if (!this.OnProbation)
                        //        {
                        //            await this.Navigation.PushModalAsync(new ThreadReplyPage(this.Thread.ThreadId, json.Id, false)).ConfigureAwait(false);
                        //        }
                        //        break;
                        //}
                    });
                    break;
                case "showUserMenu":
                    Dispatcher.Dispatch(async () =>
                    {
                        var options = !CanPM ? new string[2] { "Profile", "Their Posts" } : new string[3] { "Profile", "PM", "Their Posts" };
                        //var result = await App.Current.MainPage.DisplayActionSheet("User Options", "Cancel", null, options).ConfigureAwait(false);
                        //switch (result)
                        //{
                        //    case "Profile":
                        //        await this.Navigation.PushDetailPageAsync(new UserProfilePage(json.Id)).ConfigureAwait(false);
                        //        break;
                        //    case "PM":
                        //        await this.Navigation.PushModalAsync(new NewPrivateMessagePage(json.Text)).ConfigureAwait(false);
                        //        break;
                        //    case "Their Posts":
                        //        break;
                        //}
                    });
                    break;
            }
        }

        /// <summary>
        /// Refreshes the thread.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task RefreshThreadAsync()
        {
            if (ThreadPost != null)
            {
                IsRefreshing = true;
                await this.LoadTemplate(threadPost.ThreadId, threadPost.CurrentPage).ConfigureAwait(false);
                IsRefreshing = false;
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
            IsBusy = true;
            //var defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            //this.ThreadPost = await this.threadPostActions.GetThreadPostsAsync(threadId, pageNumber, gotoNewestPost).ConfigureAwait(false);
            //this.OnProbation = this.ThreadPost.Result.OnProbation;
            //this.OnProbationText = this.ThreadPost.Result.OnProbationText;
            //this.Title = this.ThreadPost.Name;
            //if (this.Thread != null && this.Thread.RepliesSinceLastOpened > 0)
            //{
            //    var test = ((this.ThreadPost.TotalPages - 1) * EndPoints.DefaultNumberPerPage) + this.ThreadPost.Posts.Count;
            //    this.Thread.ReplyCount = test;
            //    var seenReplies = this.Thread.ReplyCount - this.Thread.RepliesSinceLastOpened;
            //    var seenPages = seenReplies / EndPoints.DefaultNumberPerPage;
            //    if (this.ThreadPost.CurrentPage > seenPages)
            //    {
            //        var readReplies = ((this.ThreadPost.CurrentPage - (seenPages + 1)) * EndPoints.DefaultNumberPerPage) + this.ThreadPost.Posts.Count;
            //        var totalReplies = this.Thread.RepliesSinceLastOpened - readReplies;
            //        if (totalReplies < 0)
            //        {
            //            totalReplies = 0;
            //        }

            //        this.Thread.RepliesSinceLastOpened = totalReplies;
            //        this.Thread.OnPropertyChanged("RepliesSinceLastOpened");
            //    }
            //}

            //this.WebView.SetSource(this.threadPostActions.RenderThreadPostView(this.ThreadPost, defaults));
            IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();

            if (Thread != null && threadPost == null)
            {
                await this.LoadTemplate(Thread.ThreadId, 0, Thread.HasSeen).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            replyToThreadCommand?.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Mark posts as Unread.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <returns>Task.</returns>
        protected async Task MarkPostAsUnreadAsync(int postId)
        {
            var postIndex = ThreadPost.Posts.FirstOrDefault(n => n.PostId == postId);
            if (postIndex != null && Thread is not null)
            {
                var result = await threadActions.MarkPostAsLastReadAsAsync(Thread.ThreadId, postIndex.PostIndex).ConfigureAwait(false);
            }
        }
    }
}
