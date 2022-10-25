// <copyright file="ForumsListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Awful.Entities.JSON;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Tools;
using Force.DeepCloner;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Awful Forums List View Model.
    /// </summary>
    public class ForumsListViewModel : AwfulViewModel
    {
        private IndexPageActions forumActions;
        private AsyncCommand? refreshCommand;
        private AsyncCommand<object>? searchCommand;
        private List<Forum> originalList = new List<Forum>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsListViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public ForumsListViewModel(IServiceProvider services)
            : base(services)
        {
            forumActions = new IndexPageActions(Client, Context);
        }

        /// <summary>
        /// Gets the Forums Items.
        /// </summary>
        public ObservableCollection<ForumGroup> Items { get; private set; } = new ObservableCollection<ForumGroup>();

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
                        await LoadForumsAsync(Items.Count > 0).ConfigureAwait(false);
                        IsRefreshing = false;
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Search command.
        /// </summary>
        public AsyncCommand<object> SearchCommand
        {
            get
            {
                return searchCommand ??= new AsyncCommand<object>(
                    async (x) =>
                    {
                        if (x is string search)
                        {
                            FilterForums(search);
                        }
                        else
                        {
                            FilterForums(string.Empty);
                        }
                    },
                    null,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Filter the forums list, based on text.
        /// </summary>
        /// <param name="text">Text to filter by.</param>
        public void FilterForums(string text)
        {
            if (!originalList.Any())
            {
                return;
            }

            SortForumsIntoList(originalList.DeepClone(), text);
        }

        /// <summary>
        /// Loads the Forum Categories.
        /// </summary>
        /// <param name="forceReload">Force Reload.</param>
        /// <returns>Task.</returns>
        public async Task LoadForumsAsync(bool forceReload)
        {
            IsBusy = true;
            originalList = await forumActions.GetForumListAsync(forceReload).ConfigureAwait(false);
            SortForumsIntoList(originalList.DeepClone());
            IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();

            if (!Items.Any())
            {
                await RefreshCommand.ExecuteAsync().ConfigureAwait(false);
            }
        }

        private void SortForumsIntoList(List<Forum> forums, string filter = "")
        {
            Items = new ObservableCollection<ForumGroup>();
            forums = forums.Where(y => !y.HasThreads && y.ParentForumId == null).OrderBy(y => y.SortOrder).ToList();
            List<ForumGroup> items;
            if (string.IsNullOrEmpty(filter))
            {
                items = forums.Select(n => new ForumGroup(n, n.SubForums.SelectMany(n => this.Flatten(n)).OrderBy(n => n.SortOrder).ToList())).ToList();
            }
            else
            {
                items = forums.Select(n => new ForumGroup(n, n.SubForums.SelectMany(n => this.Flatten(n)).Where(n => n.Title?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).OrderBy(n => n.SortOrder).ToList())).ToList();
            }

            foreach (var item in items)
            {
                Items.Add(item);
            }

            OnPropertyChanged(nameof(Items));
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
