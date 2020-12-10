// <copyright file="PrivateMessagesPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    /// Private Message Page View Model.
    /// </summary>
    public class PrivateMessagesPageViewModel : AwfulViewModel
    {
        private PrivateMessageActions pmActions;
        private AwfulAsyncCommand refreshCommand;
        private TemplateHandler handler;
        private AwfulAsyncCommand newPMCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagesPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public PrivateMessagesPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, AwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the private message threads.
        /// </summary>
        public ObservableCollection<AwfulPM> Threads { get; set; } = new ObservableCollection<AwfulPM>();

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public AwfulAsyncCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new AwfulAsyncCommand(
                    async () =>
                {
                    await this.RefreshPMs(this.Threads.Count > 0).ConfigureAwait(false);
                },
                    () => this.CanPM,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets or sets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<AwfulPM> SelectionCommand { get; set; }

        /// <summary>
        /// Gets or sets the new pm command.
        /// </summary>
        public AwfulAsyncCommand NewPMCommand { get; set; }

        /// <summary>
        /// Refresh all PMs.
        /// </summary>
        /// <param name="forceRefresh">Force refresh PMs.</param>
        /// <returns>Task.</returns>
        public async Task RefreshPMs(bool forceRefresh = false)
        {
            this.IsRefreshing = true;
            await this.LoadThreadListAsync(forceRefresh).ConfigureAwait(false);
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Load PMs into the Thread List.
        /// </summary>
        /// <param name="forceRefresh">Force refresh PMs.</param>
        /// <returns>Task.</returns>
        public async Task LoadThreadListAsync(bool forceRefresh = false)
        {
            this.IsBusy = true;
            this.Threads.Clear();
            var threadList = await this.pmActions.GetAllPrivateMessagesAsync(forceRefresh).ConfigureAwait(false);
            foreach (var thread in threadList)
            {
                this.Threads.Add(thread);
            }

            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.NewPMCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.pmActions = new PrivateMessageActions(this.Client, this.Context, this.handler);
            if (!this.Threads.Any() && this.CanPM)
            {
                await this.RefreshCommand.ExecuteAsync().ConfigureAwait(false);
            }
        }
    }
}
