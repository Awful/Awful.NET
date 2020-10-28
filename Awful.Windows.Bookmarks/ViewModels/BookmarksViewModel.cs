// <copyright file="BookmarksViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Awful.Windows.UI.Tools.Commands;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Awful.Windows.Bookmarks.ViewModels
{
    /// <summary>
    /// Bookmarks View Model.
    /// </summary>
    public class BookmarksViewModel : AwfulViewModel
    {
        private BookmarkAction bookmarks;
        private ObservableCollection<AwfulThread> threads;

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
            var threads = await this.bookmarks.GetAllBookmarksAsync().ConfigureAwait(false);
            this.Threads = new ObservableCollection<AwfulThread>();
            foreach (var thread in threads)
            {
                this.Threads.Add(thread);
            }
        }

        public async Task RefreshBookmarksAsync()
        {
            var threads = await this.bookmarks.GetAllBookmarksAsync(true).ConfigureAwait(false);
            this.Threads = new ObservableCollection<AwfulThread>();
            foreach (var thread in threads)
            {
                this.Threads.Add(thread);
            }
        }

        public ObservableCollection<AwfulThread> Threads
        {
            get { return this.threads; }
            set { this.SetProperty(ref this.threads, value); }
        }

        private RelayCommand<object> _selectedItemCommand;
        public RelayCommand<object> SelectedItemCommand
        {
            get
            {
                return this._selectedItemCommand
                    ?? (this._selectedItemCommand = new RelayCommand<object>(async (param) =>
                    {
                        if (param is ListView listView)
                        {
                            if (listView.SelectedItem is AwfulThread thread)
                            {
                                var endpoint = string.Format(CultureInfo.InvariantCulture, EndPoints.GotoNewPostEndpoint, thread.ThreadId);
                                await Launcher.LaunchUriAsync(new Uri(endpoint));
                                await this.RefreshBookmarksAsync().ConfigureAwait(false);
                            }

                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                                listView.SelectedItem = null;
                            });
                        }
                    }));
            }
        }
    }
}
