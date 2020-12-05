// <copyright file="PrivateMessagesPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Awful.Webview;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Private Message Page View Model.
    /// </summary>
    public class PrivateMessagesPageViewModel : MobileAwfulViewModel
    {
        private PrivateMessageActions pmActions;
        private AwfulAsyncCommand refreshCommand;
        private TemplateHandler handler;
        private AwfulAsyncCommand newPMCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagesPageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public PrivateMessagesPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
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
                    this);
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<AwfulPM> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<AwfulPM>(
                    async (item) =>
                {
                    if (item != null)
                    {
                        await PushDetailPageAsync(new PrivateMessagePage(item)).ConfigureAwait(false);
                    }
                },
                    (item) => this.CanPM,
                    this);
            }
        }

        /// <summary>
        /// Gets the new pm command.
        /// </summary>
        public AwfulAsyncCommand NewPMCommand
        {
            get
            {
                return this.newPMCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        await PushModalAsync(new NewPrivateMessagePage()).ConfigureAwait(false);
                    },
                    () => !this.IsBusy && this.CanPM,
                    this);
            }
        }

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
