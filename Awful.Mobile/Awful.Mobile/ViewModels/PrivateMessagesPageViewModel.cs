// <copyright file="PrivateMessagesPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Entities.JSON;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class PrivateMessagesPageViewModel : MobileAwfulViewModel
    {
        PrivateMessageActions pmActions;
        private AwfulAsyncCommand refreshCommand;
        private TemplateHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagesPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public PrivateMessagesPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
        }

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
                    this.IsRefreshing = true;
                    await this.LoadThreadListAsync(this.Threads.Count > 0).ConfigureAwait(false);
                    this.IsRefreshing = false;
                },
                    null,
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
                    null,
                    this);
            }
        }

        /// <summary>
        /// Load PMs into the Thread List.
        /// </summary>
        /// <param name="forceRefresh">Force refresh PMs.</param>
        /// <returns>Task.</returns>
        public async Task LoadThreadListAsync(bool forceRefresh = false)
        {
            this.IsBusy = true;
            var threadList = await this.pmActions.GetAllPrivateMessagesAsync(forceRefresh).ConfigureAwait(false);
            foreach (var thread in threadList)
            {
                this.Threads.Add(thread);
            }

            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.pmActions = new PrivateMessageActions(this.Client, this.Context, this.handler);
            if (!this.Threads.Any())
            {
                await this.RefreshCommand.ExecuteAsync().ConfigureAwait(false);
            }
        }
    }
}
