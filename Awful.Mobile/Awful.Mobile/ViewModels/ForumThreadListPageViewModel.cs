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
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class ForumThreadListPageViewModel : AwfulViewModel
    {
        private ThreadListActions threadlistActions;
        private ThreadList threadList;
        private Command refreshCommand;
        private AwfulForum forum;
        private string description;

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
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
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
                    await this.RefreshForums().ConfigureAwait(false);
                });
            }
        }

        public async Task RefreshForums()
        {
            this.IsRefreshing = true;
            await this.LoadThreadListAsync(this.forum.Id, 1).ConfigureAwait(false);
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public Command<AwfulThread> SelectionCommand
        {
            get
            {
                return new Command<AwfulThread>(async (item) =>
                {
                    if (item != null)
                    {
                        await App.SetDetailPageAsync(new ForumThreadPage(item)).ConfigureAwait(false);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the new thread command.
        /// </summary>
        public Command NewThreadCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.forum != null)
                    {
                        await App.PushModalAsync(new NewThreadPage(this.forum)).ConfigureAwait(false);
                    }
                });
            }
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
            this.Description = forum.Description;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.threadlistActions = new ThreadListActions(this.Client, this.Context);
            if (!this.Threads.Any() && this.forum != null)
            {
                await this.LoadThreadListAsync(this.forum.Id, 1).ConfigureAwait(false);
            }
        }
    }
}
