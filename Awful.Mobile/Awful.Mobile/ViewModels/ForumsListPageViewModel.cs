// <copyright file="ForumsListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Entities.JSON;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.Mobile.Tools.Utilities;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Awful Forums List View Model.
    /// </summary>
    public class ForumsListPageViewModel : MobileAwfulViewModel
    {
        private IndexPageActions forumActions;
        private AwfulAsyncCommand refreshCommand;
        private AwfulAsyncCommand<AwfulForum> isFavoriteCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsListPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumsListPageViewModel(AwfulContext context)
            : base(context)
        {
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
                    await this.LoadForumsAsync(this.Items.Count > 0).ConfigureAwait(false);
                    this.IsRefreshing = false;
                },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<AwfulForum> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<AwfulForum>(
                    async (item) =>
                {
                    if (item != null)
                    {
                        await PushPageAsync(new ForumThreadListPage(item)).ConfigureAwait(false);
                    }
                },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the isFavorite command.
        /// </summary>
        public AwfulAsyncCommand<AwfulForum> IsFavoriteCommand
        {
            get
            {
                return this.isFavoriteCommand ??= new AwfulAsyncCommand<AwfulForum>(
                    async (forum) =>
                {
                    await this.forumActions.SetIsFavoriteForumAsync(forum).ConfigureAwait(false);
                    forum.OnPropertyChanged("IsFavorited");

                    var favoritedForumGroup = this.Items.FirstOrDefault(n => n.Id == 0);

                    if (favoritedForumGroup == null)
                    {
                        favoritedForumGroup = CreateFavoriteForumGroup();
                        this.Items.Insert(0, favoritedForumGroup);
                    }

                    if (forum.IsFavorited)
                    {
                        favoritedForumGroup.Add(forum);
                    }
                    else
                    {
                        favoritedForumGroup.Remove(forum);
                    }

                    if (!favoritedForumGroup.Any())
                    {
                        this.Items.Remove(favoritedForumGroup);
                    }
                },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the Forums Items.
        /// </summary>
        public ObservableCollection<ForumGroup> Items { get; private set; } = new ObservableCollection<ForumGroup>();

        /// <summary>
        /// Loads the Forum Categories.
        /// </summary>
        /// <param name="forceReload">Force Reload.</param>
        /// <returns>Task.</returns>
        public async Task LoadForumsAsync(bool forceReload)
        {
            this.Items.Clear();
            var awfulCategories = await this.forumActions.GetForumListAsync(forceReload).ConfigureAwait(false);
            awfulCategories = awfulCategories.Where(y => !y.HasThreads && y.ParentForumId == null).OrderBy(y => y.SortOrder).ToList();
            var items = awfulCategories.Select(n => new ForumGroup(n, n.SubForums.SelectMany(n => this.Flatten(n)).OrderBy(n => n.SortOrder).ToList())).ToList();
            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            var favoritedForums = items.SelectMany(y => y).Where(y => y.IsFavorited);
            if (favoritedForums.Any())
            {
                var favoritedForumGroup = CreateFavoriteForumGroup();

                foreach (var item in favoritedForums)
                {
                    favoritedForumGroup.Add(item);
                }

                this.Items.Insert(0, favoritedForumGroup);
            }

            this.OnPropertyChanged(nameof(this.Items));
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.forumActions = new IndexPageActions(this.Client, this.Context);
            if (!this.Items.Any())
            {
                await this.RefreshCommand.ExecuteAsync().ConfigureAwait(false);
            }
        }

        private static ForumGroup CreateFavoriteForumGroup()
        {
            return new ForumGroup(new Forum() { Id = 0, Title = "Favorites" }, new List<Forum>());
        }

        private IEnumerable<Forum> Flatten(Forum forum)
        {
            yield return forum;
            if (forum.SubForums != null)
            {
                foreach (var child in forum.SubForums)
                {
                    foreach (var descendant in this.Flatten(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
