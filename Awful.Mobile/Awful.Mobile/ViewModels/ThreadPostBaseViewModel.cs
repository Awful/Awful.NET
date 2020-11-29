// <copyright file="ThreadPostBaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Thread Post Base View Model.
    /// </summary>
    public class ThreadPostBaseViewModel : AwfulViewModel
    {
        private TemplateHandler handler;
        protected ThreadReplyActions replyActions;
        protected ThreadPostCreationActions postActions;

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
        /// Gets or sets the thread editor.
        /// </summary>
        public Editor Editor { get; set; }

        /// <summary>
        /// Gets the close modal command.
        /// </summary>
        public Command CloseModalCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.CloseModalAsync().ConfigureAwait(false);
                });
            }
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            this.replyActions = new ThreadReplyActions(this.Client, this.Context, this.handler);
            this.postActions = new ThreadPostCreationActions(this.Client);

            if (this.Editor != null)
            {
                this.Editor.Focus();
            }

            return base.OnLoad();
        }
    }
}
