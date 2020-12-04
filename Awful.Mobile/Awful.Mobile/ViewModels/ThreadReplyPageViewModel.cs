// <copyright file="ThreadReplyPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Core.Entities.Replies;
using Awful.Core.Entities.Web;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Thread Reply Page View Model.
    /// </summary>
    public class ThreadReplyPageViewModel : ThreadPostBaseViewModel
    {
        private ThreadReply reply;
        private int threadId = 0;
        private int id = 0;
        private bool isEdit;
        private AwfulAsyncCommand postReplyCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyPageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public ThreadReplyPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(handler, context)
        {
        }

        /// <summary>
        /// Gets the post Reply.
        /// </summary>
        public AwfulAsyncCommand PostReplyCommand
        {
            get
            {
                return this.postReplyCommand ??= new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.reply != null)
                    {
                        var replyText = this.Editor.Text.Trim();
                        if (string.IsNullOrEmpty(replyText))
                        {
                            return;
                        }

                        this.reply.MapMessage(replyText);
                        Result result;

                        if (this.isEdit)
                        {
                            result = await this.replyActions.SendUpdatePostAsync(this.reply).ConfigureAwait(false);
                        }
                        else
                        {
                            result = await this.replyActions.SendPostAsync(this.reply).ConfigureAwait(false);
                        }

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await PopModalAsync().ConfigureAwait(false);
                            await RefreshPostPageAsync().ConfigureAwait(false);
                        });
                    }
                },
                    () => this.reply != null && !string.IsNullOrEmpty(this.Message.Trim()),
                    this);
            }
        }

        /// <summary>
        /// Load thread into view.
        /// </summary>
        /// <param name="thread">Thread.</param>
        public void LoadThread(int threadId, int id = 0, bool isEdit = false)
        {
            this.threadId = threadId;
            this.id = id;
            this.isEdit = isEdit;
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.PostReplyCommand.RaiseCanExecuteChanged();
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
                    Device.BeginInvokeOnMainThread(() => this.Editor.Text = this.reply.Message);
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
                    Device.BeginInvokeOnMainThread(() => this.Editor.Text += quote);
                }

                this.IsBusy = false;
            }
        }
    }
}
