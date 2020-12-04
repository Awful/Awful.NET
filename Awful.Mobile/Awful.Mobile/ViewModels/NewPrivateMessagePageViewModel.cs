// <copyright file="NewPrivateMessagePageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Core.Entities.Messages;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Threads;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Views;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// New Private Message Page View Model.
    /// </summary>
    public class NewPrivateMessagePageViewModel : ThreadPostBaseViewModel
    {
        private PrivateMessageActions pmActions;
        private NewPrivateMessage newPrivateMessage = new NewPrivateMessage();
        private PostIcon postIcon = new PostIcon();
        private AwfulAsyncCommand postPMCommand;
        private string to = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPrivateMessagePageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public NewPrivateMessagePageViewModel(TemplateHandler handler, AwfulContext context)
            : base(handler, context)
        {
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
                            this.Popup.SetContent(new ForumPostIconSelectionView(null, this.PostIcon), true, this.OnCloseModal);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the post pm command.
        /// </summary>
        public AwfulAsyncCommand PostPMCommand
        {
            get
            {
                return this.postPMCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.newPrivateMessage != null)
                        {
                            var pmText = this.Message.Trim();
                            if (string.IsNullOrEmpty(pmText))
                            {
                                return;
                            }

                            var pmTitle = this.Subject.Trim();
                            if (string.IsNullOrEmpty(pmTitle))
                            {
                                return;
                            }

                            var to = this.To.Trim();
                            if (string.IsNullOrEmpty(to))
                            {
                                return;
                            }

                            this.newPrivateMessage.Icon = this.PostIcon;
                            this.newPrivateMessage.Title = pmTitle;
                            this.newPrivateMessage.Body = pmText;
                            this.newPrivateMessage.Receiver = to;

                            // The Manager will throw if we couldn't post.
                            // That will be captured by AwfulAsyncCommand.
                            await this.pmActions.SendPrivateMessageAsync(this.newPrivateMessage).ConfigureAwait(false);

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await PopModalAsync().ConfigureAwait(false);
                            });
                        }
                    },
                    () => this.CanPostPm,
                    this);
            }
        }

        private bool CanPostPm
        {
            get { return !string.IsNullOrEmpty(this.Subject) && !string.IsNullOrEmpty(this.To) && !string.IsNullOrEmpty(this.Message); }
        }

        /// <inheritdoc/>
        public override void OnCloseModal()
        {
            this.OnPropertyChanged(nameof(this.PostIcon));
            this.PostPMCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.PostPMCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            this.pmActions = new PrivateMessageActions(this.Client, this.Context, null);
        }
    }
}
