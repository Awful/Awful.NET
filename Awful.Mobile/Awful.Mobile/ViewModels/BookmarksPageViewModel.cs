﻿// <copyright file="BookmarksPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Bookmarks View Model.
    /// </summary>
    public class BookmarksPageViewModel : AwfulViewModel
    {
        private BookmarkAction bookmarks;
        private List<AwfulThread> threads = new List<AwfulThread>();
        private AwfulAsyncCommand refreshCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public BookmarksPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <summary>
        /// Gets or sets Forum Threads.
        /// </summary>
        public List<AwfulThread> Threads
        {
            get { return this.threads; }
            set { this.SetProperty(ref this.threads, value); }
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
                    this.IsRefreshing = true;
                    await this.RefreshBookmarksAsync().ConfigureAwait(false);
                    this.IsRefreshing = false;
                },
                    null,
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
        /// Load Bookmarks.
        /// </summary>
        /// <param name="reload">Force Reload.</param>
        /// <returns>Task.</returns>
        public async Task LoadBookmarksAsync(bool reload = false)
        {
            this.IsBusy = true;
            this.Threads = await this.bookmarks.GetAllBookmarksAsync(reload).ConfigureAwait(false);
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
        public async Task RefreshBookmarksAsync()
        {
            await this.LoadBookmarksAsync(true).ConfigureAwait(false);
        }
    }
}
