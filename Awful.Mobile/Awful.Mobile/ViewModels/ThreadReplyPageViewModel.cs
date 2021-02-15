// <copyright file="ThreadReplyPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Core.Entities.Replies;
using Awful.Core.Entities.Web;
using Awful.Database.Context;
using Awful.Database.Entities;
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
    /// Thread Reply Page View Model.
    /// </summary>
    public class ThreadReplyPageViewModel : ThreadPostBaseViewModel
    {
        protected ThreadReply reply;
        private int threadId = 0;
        private int id = 0;
        private bool isEdit;
        private AwfulAsyncCommand postReplyCommand;
        private IAwfulPopup popup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyPageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public ThreadReplyPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, handler, context)
        {
            this.popup = popup;
        }

        /// <summary>
        /// Gets a value indicating whether the user can post.
        /// </summary>
        public bool CanPost
        {
            get { return !string.IsNullOrEmpty(this.Message); }
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.PostThreadCommand?.RaiseCanExecuteChanged();
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
        /// Gets the post thread command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return this.postReplyCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.reply != null)
                        {
                            var result = await this.SendPostAsync().ConfigureAwait(false);

                            if (result == null)
                            {
                                return;
                            }

                            if (result.IsSuccess)
                            {
                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                                    await this.Navigation.RefreshPostPageAsync().ConfigureAwait(false);
                                });
                            }
                        }
                    },
                    () => this.CanPost,
                    this.Error);
            }
        }

        /// <summary>
        /// Load thread into view.
        /// </summary>
        /// <param name="threadId">Thread id.</param>
        /// <param name="id">ID of the post.</param>
        /// <param name="isEdit">Is the post an edit.</param>
        public void LoadThread(int threadId, int id = 0, bool isEdit = false)
        {
            this.threadId = threadId;
            this.id = id;
            this.isEdit = isEdit;
        }

        /// <summary>
        /// Send Post.
        /// </summary>
        /// <returns>Result.</returns>
        public async Task<Result> SendPostAsync()
        {
            if (string.IsNullOrEmpty(this.Message.Trim()))
            {
                return null;
            }

            this.reply.MapMessage(this.Message.Trim());
            if (this.isEdit)
            {
                return await this.replyActions.SendUpdatePostAsync(this.reply).ConfigureAwait(false);
            }
            else
            {
                return await this.replyActions.SendPostAsync(this.reply).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad().ConfigureAwait(false);
            if (this.reply == null)
            {
                this.IsBusy = true;

                if (this.id > 0 && this.isEdit)
                {
                    this.reply = await this.replyActions.CreateEditThreadReplyAsync(this.id).ConfigureAwait(false);
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => this.Editor.Text = this.reply.Message);
                }
                else if (this.threadId > 0)
                {
                    this.reply = await this.replyActions.CreateThreadReplyAsync(this.threadId).ConfigureAwait(false);
                }

                if (this.reply == null)
                {
                    throw new ArgumentException("Need a thread or post id!");
                }

                if (this.id > 0 && !this.isEdit)
                {
                    var quote = await this.replyActions.GetQuoteStringAsync(this.id).ConfigureAwait(false);
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => this.Editor.Text += quote);
                }

                this.IsBusy = false;
            }
        }
    }
}
