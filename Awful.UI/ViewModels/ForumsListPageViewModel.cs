// <copyright file="ForumsListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Force.DeepCloner;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Awful Forums List View Model.
    /// </summary>
    public class ForumsListPageViewModel : AwfulViewModel
    {
        private IndexPageActions forumActions;
        private AwfulAsyncCommand refreshCommand;
        private AwfulAsyncCommand<AwfulForum> isFavoriteCommand;
        private List<Forum> originalList = new List<Forum>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public ForumsListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
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
                    this.Error);
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

                    if (this.Items.Any() && this.Items.First().Title == "Search")
                    {
                        return;
                    }

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
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Forums Items.
        /// </summary>
        public ObservableCollection<ForumGroup> Items { get; private set; } = new ObservableCollection<ForumGroup>();

        /// <summary>
        /// Filter the forums list, based on text.
        /// </summary>
        /// <param name="text">Text to filter by.</param>
        public void FilterForums(string text)
        {
            if (!this.originalList.Any())
            {
                return;
            }

            this.SortForumsIntoList(this.originalList.DeepClone(), text);
        }

        /// <summary>
        /// Loads the Forum Categories.
        /// </summary>
        /// <param name="forceReload">Force Reload.</param>
        /// <returns>Task.</returns>
        public async Task LoadForumsAsync(bool forceReload)
        {
            this.originalList = await this.forumActions.GetForumListAsync(forceReload).ConfigureAwait(false);
            this.SortForumsIntoList(this.originalList.DeepClone());
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

        private void SortForumsIntoList(List<Forum> forums, string filter = "")
        {
            this.Items.Clear();
            forums = forums.Where(y => !y.HasThreads && y.ParentForumId == null).OrderBy(y => y.SortOrder).ToList();
            List<ForumGroup> items;
            if (string.IsNullOrEmpty(filter))
            {
                items = forums.Select(n => new ForumGroup(n, n.SubForums.SelectMany(n => this.Flatten(n)).OrderBy(n => n.SortOrder).ToList())).ToList();
            }
            else
            {
                items = forums.Select(n => new ForumGroup(n, n.SubForums.SelectMany(n => this.Flatten(n)).Where(n => n.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).OrderBy(n => n.SortOrder).ToList())).ToList();
            }

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
