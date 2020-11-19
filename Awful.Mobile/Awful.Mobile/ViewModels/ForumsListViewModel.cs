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
        private RelayCommand showHideForumCommand;

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
                    if (!this.IsRefreshing)
                    {
                        await this.LoadForumsAsync(true).ConfigureAwait(false);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public RelayCommand ShowHideForumCommand
        {
            get
            {
                return this.showHideForumCommand ??= new RelayCommand(async () =>
                {
                    
                });
            }
        }

        /// <summary>
        /// Gets the Forums Items.
        /// </summary>
        public List<ForumGroup> Items { get; private set; } = new List<ForumGroup>();

        /// <summary>
        /// Loads the Forum Categories.
        /// </summary>
        /// <param name="forceReload">Force Reload.</param>
        /// <returns>Task.</returns>
        public async Task LoadForumsAsync(bool forceReload)
        {
            this.IsRefreshing = true;
            var awfulCategories = await this.forumActions.GetForumListAsync(forceReload).ConfigureAwait(false);
            awfulCategories = awfulCategories.Where(y => !y.HasThreads && y.ParentForumId == null).OrderBy(y => y.SortOrder).ToList();
            this.Items = awfulCategories.Select(n => new ForumGroup(n.Title, n.SubForums.SelectMany(n => this.Flatten(n)).Where(y => !string.IsNullOrEmpty(y.Title)).OrderBy(n => n.SortOrder).ToList())).ToList();
            this.OnPropertyChanged(nameof(this.Items));
            this.IsRefreshing = false;
        }

        private IEnumerable<Forum> Flatten(Forum forum)
        {
            yield return forum;
            if (forum.SubForums != null && forum.IsShowSubForumsVisible)
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

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.forumActions = new IndexPageActions(this.Client, this.Context);
            await this.LoadForumsAsync(false).ConfigureAwait(false);
        }
    }
}
