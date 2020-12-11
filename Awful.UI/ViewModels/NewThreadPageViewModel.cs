// <copyright file="NewThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Threads;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// New Thread Page View Model.
    /// </summary>
    public class NewThreadPageViewModel : ThreadPostBaseViewModel
    {
        private NewThread newThread;
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
        public NewThreadPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, AwfulContext context)
            : base(popup, navigation, error, handler, context)
        {
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
        /// Gets or sets the SelectPostIcon Command.
        /// </summary>
        public AwfulAsyncCommand SelectPostIconCommand
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the post thread command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand { get; set; }

        private bool CanPost
        {
            get { return !string.IsNullOrEmpty(this.Subject) && !string.IsNullOrEmpty(this.Message) && !string.IsNullOrEmpty(this.postIcon.ImageEndpoint); }
        }

        /// <summary>
        /// Loads Forum into VM.
        /// </summary>
        /// <param name="forum"><see cref="AwfulForum"/>.</param>
        public void LoadForum (AwfulForum forum)
        {
            this.forum = forum;
        }

        /// <inheritdoc/>
        public override void OnCloseModal()
        {
            this.OnPropertyChanged(nameof(this.PostIcon));
            this.PostThreadCommand?.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.postThreadCommand?.RaiseCanExecuteChanged();
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
    }
}
