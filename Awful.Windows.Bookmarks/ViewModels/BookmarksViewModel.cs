// <copyright file="BookmarksViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.ViewModels;

namespace Awful.Windows.Bookmarks.ViewModels
{
    /// <summary>
    /// Bookmarks View Model.
    /// </summary>
    public class BookmarksViewModel : AwfulViewModel
    {
        private BookmarkAction bookmarks;
        private List<AwfulThread> threads;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public BookmarksViewModel(IPlatformProperties properties, AwfulContext context)
            : base(context)
        {
            this.bookmarks = new BookmarkAction(this.Client, context);
        }

        public async Task LoadBookmarksAsync()
        {
            this.threads = await this.bookmarks.GetAllBookmarksAsync().ConfigureAwait(false);
        }

        public List<AwfulThread> Threads
        {
            get { return this.threads; }
            set { this.SetProperty(ref this.threads, value); }
        }
    }
}
