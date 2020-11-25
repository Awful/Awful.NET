// <copyright file="ForumThreadListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.ViewModels;

namespace Awful.Mobile.ViewModels
{
    public class ForumThreadListViewModel : AwfulViewModel
    {
        private ThreadListActions threadlistActions;
        private ThreadList threadList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumThreadListViewModel(AwfulContext context)
            : base(context)
        {
        }

        public ObservableCollection<Thread> Threads { get; set; } = new ObservableCollection<Thread>();

        /// <summary>
        /// Load Threads into the Thread List.
        /// </summary>
        /// <param name="forumId">The Forum Id.</param>
        /// <param name="page">The Current Page.</param>
        /// <returns>Task.</returns>
        public async Task LoadThreadListAsync(int forumId, int page)
        {
            if (page == 0)
            {
                this.Threads.Clear();
            }

            this.threadList = await this.threadlistActions.GetForumThreadListAsync(forumId, page).ConfigureAwait(false);
            foreach (var thread in this.threadList.Threads)
            {
                this.Threads.Add(thread);
            }
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            this.threadlistActions = new ThreadListActions(this.Client, this.Context);
            return base.OnLoad();
        }
    }
}
