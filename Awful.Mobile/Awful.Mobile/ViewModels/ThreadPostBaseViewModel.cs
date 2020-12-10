// <copyright file="ThreadPostBaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.Mobile.Controls;
using Awful.Mobile.Views;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Thread Post Base View Model.
    /// </summary>
    public class ThreadPostBaseViewModel : MobileAwfulViewModel
    {
        protected ThreadReplyActions replyActions;
        protected ThreadPostCreationActions postActions;
        protected ThreadActions threadActions;
        private TemplateHandler handler;
        private string subject = string.Empty;
        private string message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostBaseViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public ThreadPostBaseViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the subject of the post.
        /// </summary>
        public string Subject
        {
            get
            {
                return this.subject;
            }

            set
            {
                this.SetProperty(ref this.subject, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the message of the post.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.SetProperty(ref this.message, value);
                this.RaiseCanExecuteChanged();
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
                        if (this.Popup != null)
                        {
                            if (this.Editor != null)
                            {
                                var view = new PostEditItemSelectionView(this.Editor);
                                this.Popup.SetContent(view, true);
                            }
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets or sets the thread editor.
        /// </summary>
        public IAwfulEditor Editor { get; set; }

        /// <summary>
        /// Gets the close modal command.
        /// </summary>
        public AwfulAsyncCommand CloseModalCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    await PopModalAsync().ConfigureAwait(false);
                },
                    null,
                    this);
            }
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            this.replyActions = new ThreadReplyActions(this.Client, this.Context, this.handler);
            this.postActions = new ThreadPostCreationActions(this.Client);
            this.threadActions = new ThreadActions(this.Client, this.Context);

            if (this.Editor != null)
            {
                this.Editor.Focus();
            }

            return base.OnLoad();
        }
    }
}
