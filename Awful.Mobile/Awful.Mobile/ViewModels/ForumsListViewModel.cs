// <copyright file="ForumsListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Awful.Database;
using Awful.Database.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Force.DeepCloner;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Forums List View Model.
    /// </summary>
    public class ForumsListViewModel : BaseViewModel
    {
        private IndexPageManager manager;
        private AwfulAsyncCommand<Forum> showForumsCommand;
        private AwfulAsyncCommand<Forum> favoriteForumCommand;
        private ObservableCollection<Forum> forums;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsListViewModel"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="error">Error Handler.</param>
        /// <param name="navigation">Navigation Handler.ß</param>
        public ForumsListViewModel(
            IDatabase database,
            IAwfulErrorHandler error,
            IAwfulNavigationHandler navigation)
            : base(database, error, navigation)
        {
            this.manager = new IndexPageManager(new AwfulClient());
            this.Forums = new ObservableCollection<Forum>();
        }

        /// <summary>
        /// Gets the isFavorite command.
        /// </summary>
        public AwfulAsyncCommand<Forum> FavoriteForumCommand
        {
            get
            {
                return this.favoriteForumCommand ??= new AwfulAsyncCommand<Forum>(
                    (item) =>
                    {
                        item.IsFavorited = !item.IsFavorited;
                        item.OnPropertyChanged(nameof(item.IsFavorited));
                        var category = this.Forums.FirstOrDefault(n => !n.HasThreads && n.Id == 1);
                        if (category == null)
                        {
                            category = new Forum() { Id = 1, Title = "Bookmarks", ShowSubforums = true };
                            this.Forums.Insert(0, category);
                        }

                        if (!item.IsFavorited)
                        {
                            category.SubForums.Remove(item);
                            this.Forums.Remove(item);
                        }
                        else
                        {
                            var bookmarkItem = item.DeepClone();
                            bookmarkItem.SubForums = new List<Forum>();
                            bookmarkItem.IsFavorited = item.IsFavorited;
                            category.SubForums.Add(bookmarkItem);
                            this.Forums.Insert(1, bookmarkItem);
                        }

                        if (!category.SubForums.Any())
                        {
                            this.Forums.Remove(category);
                        }

                        this.Database.SaveForums(this.Forums.ToList());
                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the isFavorite command.
        /// </summary>
        public AwfulAsyncCommand<Forum> ShowForumsCommand
        {
            get
            {
                return this.showForumsCommand ??= new AwfulAsyncCommand<Forum>(
                    (item) =>
                    {
                        item.ShowSubforums = !item.ShowSubforums;
                        item.OnPropertyChanged(nameof(item.ShowSubforums));
                        MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            this.SubForumsView(item);
                        });
                        this.Database.SaveForums(this.Forums.ToList());
                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the list of forums.
        /// </summary>
        public ObservableCollection<Forum> Forums
        {
            get => this.forums;

            set => this.SetProperty(ref this.forums, value);
        }

        /// <inheritdoc/>
        public override async Task OnAppearingAsync()
        {
            if (!this.Forums.Any())
            {
                await this.TestGetForumsList().ConfigureAwait(false);
            }
        }

        private async Task TestGetForumsList()
        {
            var cachedForums = this.Database.GetForums();
            if (!cachedForums.Any())
            {
                var result = await this.manager.GetIndexPageAsync().ConfigureAwait(false);
                foreach (var forum in result.Forums)
                {
                    forum.ShowSubforums = true;
                    this.Forums.Add(forum);
                    this.SubForumsView(forum);
                }

                this.Database.SaveForums(this.Forums.ToList());
            }
            else
            {
                foreach (var forum in cachedForums.Where(n => !n.HasThreads))
                {
                    this.Forums.Add(forum);
                    this.SubForumsView(forum);
                }
            }

            this.OnPropertyChanged(nameof(this.Forums));
        }

        private int SubForumsView(Forum item)
        {
            var index = this.Forums.IndexOf(item);
            if (item.ShowSubforums)
            {
                foreach (var forum in item.SubForums.Where(forum => forum.Id != 0))
                {
                    index += 1;
                    this.Forums.Insert(index, forum);
                    if (forum.ShowSubforums)
                    {
                        index = this.SubForumsView(forum);
                    }
                }
            }
            else
            {
                this.RemoveSubforumsFromList(item);
            }

            return index;
        }

        private void RemoveSubforumsFromList(Forum forum)
        {
            if (forum?.SubForums == null)
            {
                return;
            }

            foreach (var subForum in forum.SubForums)
            {
                var index = this.Forums.ToList().LastIndexOf(subForum);
                if (index <= -1)
                {
                    continue;
                }

                this.Forums.RemoveAt(index);
                this.RemoveSubforumsFromList(subForum);
            }
        }
    }
}