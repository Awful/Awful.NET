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
using Awful.UI.Actions;
using Awful.UI.ViewModels;
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

        public ObservableCollection<PostIcon> Icons { get; set; } = new ObservableCollection<PostIcon>();

        public void LoadPostIcon (AwfulForum forum, PostIcon icon, ThreadPostCreationActions actions)
        {
            this.forum = forum;
            this.selectedIcon = icon;
            this.threadPostCreationActions = actions;
        }

        /// <summary>
        /// Gets the close modal command.
        /// </summary>
        public Command CloseModalCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopModalAsync().ConfigureAwait(false);
                });
            }
        }

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

                    await PopModalAsync().ConfigureAwait(false);
                });
            }
        }

        public override async Task OnLoad()
        {
            if (this.forum != null && this.selectedIcon != null && this.threadPostCreationActions != null)
            {
                this.IsBusy = true;
                var icons = await this.threadPostCreationActions.GetForumPostIconsAsync(this.forum.Id).ConfigureAwait(false);
                foreach (var icon in icons)
                {
                    this.Icons.Add(icon);
                }

                this.IsBusy = false;
            }
        }
    }
}
