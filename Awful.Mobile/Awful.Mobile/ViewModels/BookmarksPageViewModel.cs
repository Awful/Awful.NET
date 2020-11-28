// <copyright file="BookmarksPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Entities.Threads;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Bookmarks View Model.
    /// </summary>
    public class BookmarksPageViewModel : AwfulViewModel
    {
        private BookmarkAction bookmarks;
        private ObservableCollection<AwfulThread> threads = new ObservableCollection<AwfulThread>();
        private Command refreshCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksPageViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public BookmarksPageViewModel(IPlatformProperties properties, AwfulContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets or sets Forum Threads.
        /// </summary>
        public ObservableCollection<AwfulThread> Threads
        {
            get { return this.threads; }
            set { this.SetProperty(ref this.threads, value); }
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
                    this.IsRefreshing = true;
                    await this.RefreshBookmarksAsync().ConfigureAwait(false);
                    this.IsRefreshing = false;
                });
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public Command<AwfulThread> SelectionCommand
        {
            get
            {
                return new Command<AwfulThread>((item) =>
                {
                    if (item != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.SetDetailPageAsync(new ForumThreadPage(item)).ConfigureAwait(false);
                        });
                    }
                });
            }
        }

        /// <summary>
        /// Load Bookmarks.
        /// </summary>
        /// <param name="reload">Force Reload.</param>
        /// <param name="forceDelay">For Reload Delay, for allowing the forum list to update.</param>
        /// <returns>Task.</returns>
        public async Task LoadBookmarksAsync(bool reload = false, int forceDelay = 0)
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            await Task.Delay(forceDelay).ConfigureAwait(false);
            var threads = await this.bookmarks.GetAllBookmarksAsync(reload).ConfigureAwait(false);
            this.Threads = new ObservableCollection<AwfulThread>();
            foreach (var thread in threads)
            {
                this.Threads.Add(thread);
            }

            if (this.Threads.Count <= 0)
            {
                this.SetState(LayoutState.Empty);
            }
            else
            {
                this.SetState(LayoutState.Custom, "SignedIn");
            }

            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            if (this.IsSignedIn)
            {
                this.bookmarks = new BookmarkAction(this.Client, this.Context);
                if (!this.Threads.Any())
                {
                    await this.LoadBookmarksAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Refresh Bookmarks.
        /// </summary>
        /// <param name="forceDelay">For Reload Delay, for allowing the forum list to update.</param>
        /// <returns>Task.</returns>
        public async Task RefreshBookmarksAsync(int forceDelay = 0)
        {
            await this.LoadBookmarksAsync(true, forceDelay).ConfigureAwait(false);
        }
    }
}
