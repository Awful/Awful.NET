// <copyright file="ForumsListViewModel.cs" company="Drastic Actions">
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
using Awful.Mobile.UI.Tools.Commands;
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
    public class ForumsListViewModel : AwfulViewModel
    {
        private IndexPageActions forumActions;
        private RelayCommand refreshCommand;
        private RelayCommand<AwfulForum> isFavoriteCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsListViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumsListViewModel(AwfulContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new RelayCommand(async () =>
                {
                    await this.LoadForumsAsync(true).ConfigureAwait(false);
                });
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public Command<AwfulForum> SelectionCommand
        {
            get
            {
                return new Command<AwfulForum>((item) =>
                {
                    if (item != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync($"forumthreadlistpage?entryId={item.Id}&title={item.Title}").ConfigureAwait(false);
                        });
                    }
                });
            }
        }

        private void RemoveForumsFromView(ForumGroup group, AwfulForum forum)
        {
            var forumIndex = group.IndexOf(forum);
            foreach (var subforum in forum.SubForums)
            {
                var temp = group.FirstOrDefault(n => n.Id == subforum.Id);
                if (temp != null)
                {
                    group.Remove(temp);

                    if (temp.SubForums.Any())
                    {
                        foreach (var t in temp.SubForums)
                        {
                            this.RemoveForumsFromView(group, (AwfulForum)t);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the isFavorite command.
        /// </summary>
        public RelayCommand<AwfulForum> IsFavoriteCommand
        {
            get
            {
                return this.isFavoriteCommand ??= new RelayCommand<AwfulForum>(async (forum) =>
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
                });
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
            this.IsRefreshing = true;
            this.Items = new ObservableCollection<ForumGroup>();
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
            this.IsRefreshing = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.forumActions = new IndexPageActions(this.Client, this.Context);
            this.IsRefreshing = true;
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
