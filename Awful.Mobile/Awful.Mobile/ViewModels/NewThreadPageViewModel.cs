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

        public string Subject { get; set; }

        public string Message { get; set; }

        public PostIcon PostIcon
        {
            get { return this.postIcon; }
            set { this.SetProperty(ref this.postIcon, value); }
        }

        public void LoadForum (AwfulForum forum)
        {
            this.forum = forum;
        }

        public Command SelectPostIconCommand
        {
            get
            {
                return new Command(async () => {
                    await App.PushModalAsync(new ForumPostIconSelectionPage(this.forum, this.PostIcon, this.postActions)).ConfigureAwait(false);
                });
            }
        }

        /// <summary>
        /// Gets the post thread command.
        /// </summary>
        public Command PostThreadCommand
        {
            get
            {
                return new Command(async () =>
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
                            await App.CloseModalAsync().ConfigureAwait(false);
                            await App.RefreshForumPage().ConfigureAwait(false);
                        });
                    }
                });
            }
        }

        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            if (this.newThread == null)
            {
                this.newThread = await this.threadActions.CreateNewThreadAsync(this.forum.Id).ConfigureAwait(false);
            }
            else
            {
                this.OnPropertyChanged("PostIcon");
            }
        }
    }
}
