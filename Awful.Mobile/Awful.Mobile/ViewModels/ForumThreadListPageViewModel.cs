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
using Awful.UI.Tools;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Forum Thread List Page View Model.
    /// </summary>
    public class ForumThreadListPageViewModel : MobileAwfulViewModel
    {
        private ThreadListActions threadlistActions;
        private ThreadList threadList;
        private AwfulAsyncCommand refreshCommand;
        private AwfulForum forum;
        private int page = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumThreadListPageViewModel(AwfulContext context)
            : base(context)
        {
        }

        public ObservableCollection<AwfulThread> Threads { get; set; } = new ObservableCollection<AwfulThread>();

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
                    this);
            }
        }

        /// <summary>
        /// Gets the next page command.
        /// </summary>
        public AwfulAsyncCommand NextPageCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (!this.IsBusy && this.threadList != null && this.page <= this.threadList.TotalPages)
                    {
                        this.page++;
                        await this.LoadThreadListAsync(this.forum.Id, this.page).ConfigureAwait(false);
                    }
                },
                    null,
                    this);
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
                        await PushDetailPageAsync(new ForumThreadPage(item)).ConfigureAwait(false);
                    }
                },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the new thread command.
        /// </summary>
        public AwfulAsyncCommand NewThreadCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.forum != null)
                    {
                        await PushModalAsync(new NewThreadPage(this.forum)).ConfigureAwait(false);
                    }
                },
                    null,
                    this);
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

            if (page == 1)
            {
                this.Threads.Clear();
            }

            this.IsBusy = true;
            this.threadList = await this.threadlistActions.GetForumThreadListAsync(forumId, page).ConfigureAwait(false);
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
