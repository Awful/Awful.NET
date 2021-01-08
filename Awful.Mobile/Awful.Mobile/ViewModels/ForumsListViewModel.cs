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
                        var index = this.Forums.IndexOf(item);
                        item.ShowSubforums = !item.ShowSubforums;
                        MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            if (item.ShowSubforums)
                            {
                                foreach (var forum in item.SubForums.Where(forum => forum.Id != 0))
                                {
                                    index = index + 1;
                                    this.Forums.Insert(index, forum);
                                }

                                //this.Forums.InsertRange(index + 1, item.SubForums);
                            }
                            else
                            {
                                this.RemoveSubforumsFromList(item);
                            }

                            this.OnPropertyChanged(nameof(this.Forums));
                            return Task.CompletedTask;
                        });
                        return Task.CompletedTask;
                },
                    null,
                    this.Error);
            }
        }

        public ObservableCollection<Forum> Forums
        {
            get
            {
                return this.forums;
            }

            set
            {
                this.SetProperty(ref this.forums, value);
            }
        }

        /// <inheritdoc/>
        public override async Task OnAppearingAsync()
        {
            if (!this.Forums.Any())
            {
                await this.TestGetForumsList().ConfigureAwait(false);
            }

            this.OnPropertyChanged(nameof(this.Forums));
        }

        private async Task TestGetForumsList()
        {
            var result = await this.manager.GetIndexPageAsync().ConfigureAwait(false);
            foreach (var forum in result.Forums)
            {
                this.Forums.Add(forum);
            }
        }

        private void RemoveSubforumsFromList(Forum forum)
        {
            if (forum?.SubForums == null)
            {
                return;
            }

            foreach (var subforum in forum.SubForums)
            {
                this.Forums.Remove(subforum);
                this.RemoveSubforumsFromList(subforum);
            }
        }
    }
}