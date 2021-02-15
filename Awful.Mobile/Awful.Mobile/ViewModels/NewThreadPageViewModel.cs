// <copyright file="NewThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Threads;
using Awful.Core.Entities.Web;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Views;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// New Thread Page View Model.
    /// </summary>
    public class NewThreadPageViewModel : ThreadPostBaseViewModel
    {
        protected NewThread newThread;
        private IAwfulPopup popup;
        private AwfulForum forum;
        private PostIcon postIcon = new PostIcon();
        private AwfulAsyncCommand postThreadCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public NewThreadPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, handler, context)
        {
            this.popup = popup;
        }

        /// <summary>
        /// Gets or sets the Post Icon.
        /// </summary>
        public PostIcon PostIcon
        {
            get
            {
                return this.postIcon;
            }

            set
            {
                this.SetProperty(ref this.postIcon, value);
            }
        }

        /// <summary>
        /// Gets the options command.
        /// </summary>
        public AwfulAsyncCommand OpenOptionsCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    () =>
                    {
                        if (this.popup != null)
                        {
                            if (this.Editor != null)
                            {
                                var view = new PostEditItemSelectionView(this.Editor);
                                this.popup.SetContent(view, true);
                            }
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
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
                        if (this.popup != null)
                        {
                            this.popup.SetContent(new ForumPostIconSelectionView(null, this.PostIcon), true, this.OnCloseModal);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the post thread command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return this.postThreadCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.newThread != null)
                        {

                            var result = await this.PostNewThreadAsync().ConfigureAwait(false);

                            // If we get a null result, we couldn't post at all. Ignore.
                            // TODO: It shouldn't ever return null anyway because of the Command Check.
                            // We probably don't need this check.
                            if (result == null)
                            {
                                return;
                            }

                            if (result.IsSuccess)
                            {
                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                                    await this.Navigation.RefreshForumPageAsync().ConfigureAwait(false);
                                });
                            }
                        }
                    },
                    () => this.CanPost,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can post.
        /// </summary>
        public bool CanPost
        {
            get { return !string.IsNullOrEmpty(this.Subject) && !string.IsNullOrEmpty(this.Message) && !string.IsNullOrEmpty(this.postIcon.ImageEndpoint); }
        }

        /// <summary>
        /// Loads Forum into VM.
        /// </summary>
        /// <param name="forum"><see cref="AwfulForum"/>.</param>
        public void LoadForum(AwfulForum forum)
        {
            this.forum = forum;
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

        /// <inheritdoc/>
        public override void OnCloseModal()
        {
            this.OnPropertyChanged(nameof(this.PostIcon));
            this.PostThreadCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.PostThreadCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Posts the thread to the forums.
        /// </summary>
        /// <returns>A Result.</returns>
        public async Task<Result> PostNewThreadAsync()
        {
            var threadText = this.Message.Trim();
            if (string.IsNullOrEmpty(threadText))
            {
                return null;
            }

            var threadTitle = this.Subject.Trim();
            if (string.IsNullOrEmpty(threadTitle))
            {
                return null;
            }

            if (string.IsNullOrEmpty(this.PostIcon.ImageLocation))
            {
                return null;
            }

            this.newThread.PostIcon = this.PostIcon;
            this.newThread.Subject = threadTitle;
            this.newThread.Content = threadText;

            // The Manager will throw if we couldn't post.
            // That will be captured by AwfulAsyncCommand.
            return await this.threadActions.PostNewThreadAsync(this.newThread).ConfigureAwait(false);
        }
    }
}
