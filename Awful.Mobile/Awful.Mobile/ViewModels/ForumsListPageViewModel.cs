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
using Awful.Mobile.Pages;
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
        private AwfulAsyncCommand<string> searchCommand;
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
        /// Gets the refresh command.
        /// </summary>
        public AwfulAsyncCommand<string> SearchCommand
        {
            get
            {
                return this.searchCommand ??= new AwfulAsyncCommand<string>(
                    async (x) => this.FilterForums(x),
                    null,
                    this.Error);
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
                            await this.Navigation.PushPageAsync(new ForumThreadListPage(item)).ConfigureAwait(false);
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
            this.IsBusy = true;
            this.originalList = await this.forumActions.GetForumListAsync(forceReload).ConfigureAwait(false);
            this.SortForumsIntoList(this.originalList.DeepClone());
            this.IsBusy = false;
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
            this.Items = new ObservableCollection<ForumGroup>();
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

            this.OnPropertyChanged(nameof(this.Items));
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
