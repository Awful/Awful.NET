// <copyright file="ThreadPostBaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Thread Post Base View Model.
    /// </summary>
    public class ThreadPostBaseViewModel : AwfulViewModel
    {
        protected ThreadReplyActions replyActions;
        protected ThreadPostCreationActions postActions;
        protected ThreadActions threadActions;
        private ITemplateHandler handler;
        private string subject = string.Empty;
        private string message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostBaseViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful handler.</param>
        /// <param name="context">Awful Context.</param>
        public ThreadPostBaseViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
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
                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                },
                    null,
                    this.Error);
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
