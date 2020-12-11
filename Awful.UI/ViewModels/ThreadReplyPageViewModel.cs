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
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.UI.ViewModels
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
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public ThreadReplyPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(popup, navigation, error, handler, context)
        {
        }

        /// <summary>
        /// Gets or sets the post Reply.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand { get; set; }

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

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.postReplyCommand?.RaiseCanExecuteChanged();
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
