// <copyright file="ForumThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.Threads;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Forum Thread Page View Model.
    /// </summary>
    public class ForumThreadPageViewModel : AwfulViewModel
    {
        private TemplateHandler handler;
        private ThreadPostActions threadActions;
        private ThreadPost threadPost;
        private Command refreshCommand;
        private bool selfInvoked;
        private AwfulThread thread;
        private DefaultOptions defaults;
        private DefaultOptions previousDefaults;

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
        /// Gets or sets the internal webview.
        /// </summary>
        public WebView WebView { get; set; }

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
                    if (this.ThreadPost != null)
                    {
                        this.IsRefreshing = true;
                        await this.LoadTemplate(this.threadPost.ThreadId, this.threadPost.CurrentPage).ConfigureAwait(false);
                        this.IsRefreshing = false;
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
        /// <returns>Task.</returns>
        public async Task LoadTemplate(int threadId, int pageNumber, bool gotoNewestPost = false)
        {
            this.IsBusy = true;
            this.defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            this.ThreadPost = await this.threadActions.GetThreadPostsAsync(threadId, pageNumber, gotoNewestPost).ConfigureAwait(false);
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
            source.Html = this.threadActions.RenderThreadPostView(this.ThreadPost, defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
            await Task.Delay(2000).ConfigureAwait(false);
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
            this.threadActions = new ThreadPostActions(this.Client, this.Context, this.handler);
            if (this.thread != null && this.threadPost == null)
            {
                await this.LoadTemplate(this.thread.ThreadId, 0, this.thread.HasBeenViewed).ConfigureAwait(false);
            }
        }
    }
}
