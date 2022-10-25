// <copyright file="ForumPostIconSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.PostIcons;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Tools;
using System.Collections.ObjectModel;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Forum Post Icon Selection View Model.
    /// </summary>
    public class ForumPostIconSelectionViewModel : AwfulViewModel
    {
        private AwfulForum forum;
        private ThreadPostCreationActions threadPostCreationActions;
        private AsyncCommand<PostIcon>? selectedPostCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostIconSelectionViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public ForumPostIconSelectionViewModel(AwfulForum forum, IServiceProvider services)
            : base(services)
        {
            this.forum = forum;
            threadPostCreationActions = new ThreadPostCreationActions(Client);
        }

        /// <summary>
        /// Gets the thread post icons.
        /// </summary>
        public ObservableCollection<PostIcon> Icons { get; internal set; } = new ObservableCollection<PostIcon>();

        /// <summary>
        /// Gets the close modal command.
        /// </summary>
        public AsyncCommand<PostIcon> SelectPostIconCommand
        {
            get
            {
                return selectedPostCommand ??= new AsyncCommand<PostIcon>(
                    async (icon) =>
                    {
                        //this.selectedIcon.Id = icon.Id;
                        //this.selectedIcon.ImageEndpoint = icon.ImageEndpoint;
                        //this.selectedIcon.ImageLocation = icon.ImageLocation;
                        //this.selectedIcon.Title = icon.Title;

                        //if (this.popup != null)
                        //{
                        //    this.popup.SetIsVisible(false);
                        //}
                        //else
                        //{
                        //    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                        //}
                    },
                    null,
                    ErrorHandler);
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();

            if (threadPostCreationActions != null)
            {
                IsBusy = true;
                Icons.Clear();
                PostIconList icons;
                if (forum == null)
                {
                    icons = await threadPostCreationActions.GetPrivateMessagePostIconsAsync().ConfigureAwait(false);
                }
                else
                {
                    icons = await threadPostCreationActions.GetForumPostIconsAsync(forum.Id).ConfigureAwait(false);
                }

                foreach (var icon in icons.Icons)
                {
                    Icons.Add(icon);
                }

                IsBusy = false;
            }
        }
    }
}
