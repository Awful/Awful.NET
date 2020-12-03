// <copyright file="NewThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Tools;
using Awful.Mobile.Views;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// New Thread Page View Model.
    /// </summary>
    public class NewThreadPageViewModel : ThreadPostBaseViewModel
    {
        private NewThread newThread;
        private AwfulForum forum;
        private PostIcon postIcon = new PostIcon();

        /// <summary>
        /// Initializes a new instance of the <see cref="NewThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public NewThreadPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(handler, context)
        {
        }

        /// <summary>
        /// Gets or sets the subject of the post.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message of the post.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Post Icon.
        /// </summary>
        public PostIcon PostIcon
        {
            get { return this.postIcon; }
            set { this.SetProperty(ref this.postIcon, value); }
        }

        /// <summary>
        /// Gets the SelectPostIcon Command.
        /// </summary>
        public AwfulAsyncCommand SelectPostIconCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    () =>
                    {
                        if (this.Popup != null)
                        {
                            this.Popup.SetContent(new ForumPostIconSelectionView(this.forum, this.PostIcon), true, this.OnCloseModal);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the post thread command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.newThread != null)
                        {
                            var threadText = this.Message.Trim();
                            if (string.IsNullOrEmpty(threadText))
                            {
                                return;
                            }

                            var threadTitle = this.Title.Trim();
                            if (string.IsNullOrEmpty(threadTitle))
                            {
                                return;
                            }

                            if (string.IsNullOrEmpty(this.PostIcon.ImageLocation))
                            {
                                return;
                            }

                            this.newThread.PostIcon = this.PostIcon;
                            this.newThread.Subject = threadTitle;
                            this.newThread.Content = threadText;
                            Result result;

                            result = await this.threadActions.PostNewThreadAsync(this.newThread).ConfigureAwait(false);

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await PopModalAsync().ConfigureAwait(false);
                                await RefreshForumPageAsync().ConfigureAwait(false);
                            });
                        }
                    },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Loads Forum into VM.
        /// </summary>
        /// <param name="forum"><see cref="AwfulForum"/>.</param>
        public void LoadForum (AwfulForum forum)
        {
            this.forum = forum;
        }

        /// <inheritdoc/>
        public override void OnCloseModal()
        {
            this.OnPropertyChanged(nameof(this.PostIcon));
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            if (this.newThread == null)
            {
                this.newThread = await this.threadActions.CreateNewThreadAsync(this.forum.Id).ConfigureAwait(false);
            }
            else
            {
                this.OnPropertyChanged(nameof(this.PostIcon));
            }
        }
    }
}
