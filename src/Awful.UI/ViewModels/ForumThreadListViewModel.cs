// <copyright file="ForumThreadListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.Threads;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Forum Thread List View Model.
    /// </summary>
    public class ForumThreadListViewModel : AwfulViewModel
    {
        protected AwfulForum forum;
        protected AsyncCommand? newThreadCommand;
        private ThreadListActions threadlistActions;
        private ThreadList? threadList;
        private AsyncCommand? refreshCommand;
        private AsyncCommand? firstPageCommand;
        private AsyncCommand? lastPageCommand;
        private AsyncCommand? nextPageCommand;
        private AsyncCommand? previousPageCommand;
        private int page = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public ForumThreadListViewModel(AwfulForum forum, IServiceProvider services)
            : base(services)
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            this.forum = forum;
            Title = forum.Title ?? "Missing Title";
            threadlistActions = new ThreadListActions(Client, Context);
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
            get { return page; }
            set { SetProperty(ref page, value); RaiseCanExecuteChanged(); }
        }

        /// <summary>
        /// Gets the total page.
        /// </summary>
        public int TotalPages
        {
            get { return threadList != null ? threadList.TotalPages : 1; }
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
                        await RefreshForums();
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the new thread command.
        /// </summary>
        public AsyncCommand NewThreadCommand
        {
            get
            {
                return newThreadCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (forum != null)
                        {
                            //  await this.Navigation.PushModalAsync(new NewThreadPage(this.forum));
                        }
                    },
                    () => !IsBusy && !OnProbation,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AsyncCommand<AwfulThread> SelectionCommand
        {
            get
            {
                return new AsyncCommand<AwfulThread>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            // await this.Navigation.PushDetailPageAsync(new ForumThreadPage(item));
                        }
                    },
                    null,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the previous page command.
        /// </summary>
        public AsyncCommand FirstPageCommand
        {
            get
            {
                return firstPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (!IsBusy && threadList != null && Page <= threadList.TotalPages)
                        {
                            Page = 1;
                            await this.LoadThreadListAsync(forum.Id, Page);
                        }
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the previous page command.
        /// </summary>
        public AsyncCommand LastPageCommand
        {
            get
            {
                return lastPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (!IsBusy && threadList != null && Page <= threadList.TotalPages)
                        {
                            Page = threadList.TotalPages;
                            await this.LoadThreadListAsync(forum.Id, Page);
                        }
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the previous page command.
        /// </summary>
        public AsyncCommand PreviousPageCommand
        {
            get
            {
                return previousPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (!IsBusy && threadList != null && Page <= threadList.TotalPages)
                        {
                            Page--;
                            await this.LoadThreadListAsync(forum.Id, Page);
                        }
                    },
                    () => Page > 1,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the next page command.
        /// </summary>
        public AsyncCommand NextPageCommand
        {
            get
            {
                return nextPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (!IsBusy && threadList != null && Page <= threadList.TotalPages)
                        {
                            Page++;
                            await this.LoadThreadListAsync(forum.Id, Page);
                        }
                    },
                    () => Page < TotalPages,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Reset forum thread list and reload it.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RefreshForums()
        {
            IsRefreshing = true;
            await this.LoadThreadListAsync(forum.Id, 1);
            IsRefreshing = false;
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

            Threads.Clear();

            IsBusy = true;
            threadList = await threadlistActions.GetForumThreadListAsync(forumId, page);
            if (threadList.Result is not null)
            {
                OnProbation = threadList.Result.OnProbation;
                OnProbationText = threadList.Result.OnProbationText ?? string.Empty;
            }

            foreach (var thread in threadList.Threads)
            {
                Threads.Add(new AwfulThread(thread));
            }

            IsBusy = false;
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            newThreadCommand?.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
        }
    }
}
