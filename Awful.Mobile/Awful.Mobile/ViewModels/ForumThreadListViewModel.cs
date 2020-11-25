// <copyright file="ForumThreadListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.UI.Tools.Commands;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class ForumThreadListViewModel : AwfulViewModel
    {
        private ThreadListActions threadlistActions;
        private ThreadList threadList;
        private RelayCommand refreshCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumThreadListViewModel(AwfulContext context)
            : base(context)
        {
        }

        public int ForumId { get; set; }

        public ObservableCollection<AwfulThread> Threads { get; set; } = new ObservableCollection<AwfulThread>();

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new RelayCommand(async () =>
                {
                    await this.LoadThreadListAsync(this.ForumId, 0).ConfigureAwait(false);
                });
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public Command<Thread> SelectionCommand
        {
            get
            {
                return new Command<Thread>((item) =>
                {
                    if (item != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync($"forumthreadpage?entryId={item.ThreadId}&title={item.Name}").ConfigureAwait(false);
                        });
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

            if (page == 0)
            {
                this.Threads.Clear();
            }

            this.IsRefreshing = true;
            this.threadList = await this.threadlistActions.GetForumThreadListAsync(forumId, page).ConfigureAwait(false);
            foreach (var thread in this.threadList.Threads)
            {
                this.Threads.Add(new AwfulThread(thread));
            }

            this.IsRefreshing = false;
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            this.threadlistActions = new ThreadListActions(this.Client, this.Context);
            if (!this.Threads.Any())
            {
                this.IsRefreshing = true;
            }

            return base.OnLoad();
        }
    }
}
