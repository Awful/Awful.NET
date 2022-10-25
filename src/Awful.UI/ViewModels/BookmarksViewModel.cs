// <copyright file="BookmarksViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Tools;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Bookmarks View Model.
    /// </summary>
    public class BookmarksViewModel : AwfulViewModel
    {
        private BookmarkAction bookmarks;
        private List<AwfulThread> threads = new List<AwfulThread>();
        private AsyncCommand? refreshCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public BookmarksViewModel(IServiceProvider services)
            : base(services)
        {
            bookmarks = new BookmarkAction(Client, Context);
        }

        /// <summary>
        /// Gets or sets Forum Threads.
        /// </summary>
        public List<AwfulThread> Threads
        {
            get { return threads; }
            set { SetProperty(ref threads, value); }
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
                        IsRefreshing = true;
                        await RefreshBookmarksAsync().ConfigureAwait(false);
                        IsRefreshing = false;
                    },
                    null,
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
                            // await this.Navigation.PushDetailPageAsync(new ForumThreadPage(item)).ConfigureAwait(false);
                        }
                    },
                    null,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Load Bookmarks.
        /// </summary>
        /// <param name="reload">Force Reload.</param>
        /// <returns>Task.</returns>
        public async Task LoadBookmarksAsync(bool reload = false)
        {
            IsBusy = true;
            Threads = await bookmarks.GetAllBookmarksAsync(reload).ConfigureAwait(false);
            IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();

            if (IsSignedIn)
            {
                if (!Threads.Any())
                {
                    await LoadBookmarksAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Refresh Bookmarks.
        /// </summary>
        /// <param name="forceDelay">For Reload Delay, for allowing the forum list to update.</param>
        /// <returns>Task.</returns>
        public async Task RefreshBookmarksAsync()
        {
            await LoadBookmarksAsync(true);
        }
    }
}
