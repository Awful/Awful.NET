// <copyright file="ForumThreadListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Forum Thread List Page View Model.
    /// </summary>
    public class ForumThreadListPageViewModel : AwfulViewModel
    {
        protected AwfulForum forum;
        protected AwfulAsyncCommand newThreadCommand;
        private ThreadListActions threadlistActions;
        private ThreadList threadList;
        private AwfulAsyncCommand refreshCommand;
        private AwfulAsyncCommand firstPageCommand;
        private AwfulAsyncCommand lastPageCommand;
        private AwfulAsyncCommand nextPageCommand;
        private AwfulAsyncCommand previousPageCommand;
        private int page = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public ForumThreadListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <summary>
        /// Gets the list of threads.
        /// </summary>
        public ObservableCollection<AwfulThread> Threads { get; set; } = new ObservableCollection<AwfulThread>();

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        public int Page
        {
            get { return this.page; }
            set { this.SetProperty(ref this.page, value); this.RaiseCanExecuteChanged(); }
        }

        /// <summary>
        /// Gets the total page.
        /// </summary>
        public int TotalPages
        {
            get { return this.threadList != null ? this.threadList.TotalPages : 1; }
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
                    await this.RefreshForums().ConfigureAwait(false);
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the new thread command.
        /// </summary>
        public AwfulAsyncCommand NewThreadCommand
        {
            get
            {
                return this.newThreadCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.forum != null)
                        {
                            await this.Navigation.PushModalAsync(new NewThreadPage(this.forum)).ConfigureAwait(false);
                        }
                    },
                    () => !this.IsBusy && !this.OnProbation,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<AwfulThread> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<AwfulThread>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            await this.Navigation.PushDetailPageAsync(new ForumThreadPage(item)).ConfigureAwait(false);
                        }
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the previous page command.
        /// </summary>
        public AwfulAsyncCommand FirstPageCommand
        {
            get
            {
                return this.firstPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (!this.IsBusy && this.threadList != null && this.Page <= this.threadList.TotalPages)
                        {
                            this.Page = 1;
                            await this.LoadThreadListAsync(this.forum.Id, this.Page).ConfigureAwait(false);
                        }
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the previous page command.
        /// </summary>
        public AwfulAsyncCommand LastPageCommand
        {
            get
            {
                return this.lastPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (!this.IsBusy && this.threadList != null && this.Page <= this.threadList.TotalPages)
                        {
                            this.Page = this.threadList.TotalPages;
                            await this.LoadThreadListAsync(this.forum.Id, this.Page).ConfigureAwait(false);
                        }
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the previous page command.
        /// </summary>
        public AwfulAsyncCommand PreviousPageCommand
        {
            get
            {
                return this.previousPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (!this.IsBusy && this.threadList != null && this.Page <= this.threadList.TotalPages)
                        {
                            this.Page--;
                            await this.LoadThreadListAsync(this.forum.Id, this.Page).ConfigureAwait(false);
                        }
                    },
                    () => this.Page > 1,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the next page command.
        /// </summary>
        public AwfulAsyncCommand NextPageCommand
        {
            get
            {
                return this.nextPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                {
                    if (!this.IsBusy && this.threadList != null && this.Page <= this.threadList.TotalPages)
                    {
                        this.Page++;
                        await this.LoadThreadListAsync(this.forum.Id, this.Page).ConfigureAwait(false);
                    }
                },
                    () => this.Page < this.TotalPages,
                    this.Error);
            }
        }

        /// <summary>
        /// Reset forum thread list and reload it.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RefreshForums()
        {
            this.IsRefreshing = true;
            await this.LoadThreadListAsync(this.forum.Id, 1).ConfigureAwait(false);
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Load Threads into the Thread List.
        /// </summary>
        /// <param name="forumId">The Forum Id.</param>
        /// <param name="page">The Current Page.</param>
        /// <returns>Task.</returns>
        public async Task LoadThreadListAsync(int forumId, int page)
        {
            if (forumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(forumId));
            }

            this.Threads.Clear();

            this.IsBusy = true;
            this.threadList = await this.threadlistActions.GetForumThreadListAsync(forumId, page).ConfigureAwait(false);
            this.OnProbation = this.threadList.Result.OnProbation;
            this.OnProbationText = this.threadList.Result.OnProbationText;
            foreach (var thread in this.threadList.Threads)
            {
                this.Threads.Add(new AwfulThread(thread));
            }

            this.IsBusy = false;
        }

        /// <summary>
        /// Load Forum.
        /// </summary>
        /// <param name="forum">Forum to load.</param>
        public void LoadForum(AwfulForum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            this.forum = forum;
            this.Title = forum.Title;
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.newThreadCommand?.RaiseCanExecuteChanged();
            this.NextPageCommand.RaiseCanExecuteChanged();
            this.PreviousPageCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.threadlistActions = new ThreadListActions(this.Client, this.Context);
            if (!this.Threads.Any() && this.forum != null)
            {
                await this.RefreshCommand.ExecuteAsync().ConfigureAwait(false);
            }
        }
    }
}
