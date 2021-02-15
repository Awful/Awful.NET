// <copyright file="NewPrivateMessagePageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Core.Entities;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Web;
using Awful.Database.Context;
using Awful.Mobile.Views;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Essentials;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// New Private Message Page View Model.
    /// </summary>
    public class NewPrivateMessagePageViewModel : ThreadPostBaseViewModel
    {
        protected NewPrivateMessage newPrivateMessage = new NewPrivateMessage();
        private PrivateMessageActions pmActions;
        private PostIcon postIcon = new PostIcon();
        private string to = string.Empty;
        private IAwfulPopup popup;
        private AwfulAsyncCommand postPMCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPrivateMessagePageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public NewPrivateMessagePageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, handler, context)
        {
            this.popup = popup;
        }

        /// <summary>
        /// Gets or sets the subject of the post.
        /// </summary>
        public string To
        {
            get
            {
                return this.to;
            }

            set
            {
                this.SetProperty(ref this.to, value);
                this.RaiseCanExecuteChanged();
            }
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
        /// Gets a value indicating whether the user can post a pm.
        /// </summary>
        public bool CanPostPm
        {
            get { return !string.IsNullOrEmpty(this.Subject) && !string.IsNullOrEmpty(this.To) && !string.IsNullOrEmpty(this.Message); }
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
        /// Gets the post pm command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return this.postPMCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.newPrivateMessage != null)
                        {
                            var saItem = await this.SendPrivateMessageAsync().ConfigureAwait(false);

                            // If we get a null result, we couldn't post at all. Ignore.
                            // TODO: It shouldn't ever return null anyway because of the Command Check.
                            // We probably don't need this check.
                            if (saItem == null)
                            {
                                return;
                            }

                            if (saItem.IsResultSet && saItem.Result.IsSuccess)
                            {
                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                                });
                            }
                        }
                    },
                    () => this.CanPostPm,
                    this.Error);
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

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            this.pmActions = new PrivateMessageActions(this.Client, this.Context, null);
        }

        /// <summary>
        /// Sends the private message.
        /// </summary>
        /// <returns>Result.</returns>
        public async Task<SAItem> SendPrivateMessageAsync()
        {
            var pmText = this.Message.Trim();
            if (string.IsNullOrEmpty(pmText))
            {
                return null;
            }

            var pmTitle = this.Subject.Trim();
            if (string.IsNullOrEmpty(pmTitle))
            {
                return null;
            }

            var to = this.To.Trim();
            if (string.IsNullOrEmpty(to))
            {
                return null;
            }

            this.newPrivateMessage.Icon = this.PostIcon;
            this.newPrivateMessage.Title = pmTitle;
            this.newPrivateMessage.Body = pmText;
            this.newPrivateMessage.Receiver = to;

            // The Manager will throw if we couldn't post.
            // That will be captured by AwfulAsyncCommand.
            return await this.pmActions.SendPrivateMessageAsync(this.newPrivateMessage).ConfigureAwait(false);
        }
    }
}
