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

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public ThreadReplyPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, handler, context)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the user can post.
        /// </summary>
        public bool CanPost
        {
            get { return !string.IsNullOrEmpty(this.Message); }
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
