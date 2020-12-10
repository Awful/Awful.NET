// <copyright file="ForumPostIconSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Forum Post Icon Selection View Model.
    /// </summary>
    public class ForumPostIconSelectionViewModel : AwfulViewModel
    {
        private AwfulForum forum;
        private PostIcon selectedIcon;
        private ThreadPostCreationActions threadPostCreationActions;
        private IAwfulPopup popup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostIconSelectionViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public ForumPostIconSelectionViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, AwfulContext context)
            : base(navigation, error, context)
        {
            this.popup = popup;
        }

        /// <summary>
        /// Gets the thread post icons.
        /// </summary>
        public ObservableCollection<PostIcon> Icons { get; internal set; } = new ObservableCollection<PostIcon>();

        /// <summary>
        /// Gets the close modal command.
        /// </summary>
        public AwfulAsyncCommand<PostIcon> SelectPostIconCommand
        {
            get
            {
                return new AwfulAsyncCommand<PostIcon>(
                    async (icon) =>
                {
                    this.selectedIcon.Id = icon.Id;
                    this.selectedIcon.ImageEndpoint = icon.ImageEndpoint;
                    this.selectedIcon.ImageLocation = icon.ImageLocation;
                    this.selectedIcon.Title = icon.Title;

                    if (this.popup != null)
                    {
                        this.popup.SetIsVisible(false);
                    }
                    else
                    {
                        await this.Navigation.PopModalAsync().ConfigureAwait(false);
                    }
                },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Load Post Icon into VM.
        /// </summary>
        /// <param name="forum"><see cref="AwfulForum"/> to load.</param>
        /// <param name="icon">Original PostIcon from view.</param>
        public void LoadPostIcon(AwfulForum forum, PostIcon icon)
        {
            this.forum = forum;
            this.selectedIcon = icon;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.threadPostCreationActions = new ThreadPostCreationActions(this.Client);
            if (this.selectedIcon != null && this.threadPostCreationActions != null)
            {
                this.IsBusy = true;
                this.Icons.Clear();
                PostIconList icons;
                if (this.forum == null)
                {
                    icons = await this.threadPostCreationActions.GetPrivateMessagePostIconsAsync().ConfigureAwait(false);
                }
                else
                {
                    icons = await this.threadPostCreationActions.GetForumPostIconsAsync(this.forum.Id).ConfigureAwait(false);
                }

                foreach (var icon in icons.Icons)
                {
                    this.Icons.Add(icon);
                }

                this.IsBusy = false;
            }
        }
    }
}
