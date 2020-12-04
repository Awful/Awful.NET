// <copyright file="ForumPostIconSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Forms9Patch;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Forum Post Icon Selection View Model.
    /// </summary>
    public class ForumPostIconSelectionViewModel : MobileAwfulViewModel
    {
        private AwfulForum forum;
        private PostIcon selectedIcon;
        private ThreadPostCreationActions threadPostCreationActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostIconSelectionViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public ForumPostIconSelectionViewModel(AwfulContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets the thread post icons.
        /// </summary>
        public ObservableCollection<PostIcon> Icons { get; internal set; } = new ObservableCollection<PostIcon>();

        /// <summary>
        /// Gets the close modal command.
        /// </summary>
        public Command<PostIcon> SelectPostIconCommand
        {
            get
            {
                return new Command<PostIcon>(async (icon) =>
                {
                    this.selectedIcon.Id = icon.Id;
                    this.selectedIcon.ImageEndpoint = icon.ImageEndpoint;
                    this.selectedIcon.ImageLocation = icon.ImageLocation;
                    this.selectedIcon.Title = icon.Title;

                    if (this.Popup != null)
                    {
                        this.Popup.SetIsVisible(false);
                    }
                    else
                    {
                        await PopModalAsync().ConfigureAwait(false);
                    }
                });
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
