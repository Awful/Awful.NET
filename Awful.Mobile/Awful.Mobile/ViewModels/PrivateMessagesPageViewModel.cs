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
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class PrivateMessagesPageViewModel : AwfulViewModel
    {
        PrivateMessageActions pmActions;
        private Command refreshCommand;
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
        public Command RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new Command(async () =>
                {
                    this.IsRefreshing = true;
                    await this.LoadThreadListAsync(true).ConfigureAwait(false);
                    this.IsRefreshing = false;
                });
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public Command<AwfulPM> SelectionCommand
        {
            get
            {
                return new Command<AwfulPM>(async (item) =>
                {
                    if (item != null)
                    {
                        await App.SetDetailPageAsync(new PrivateMessagePage(item)).ConfigureAwait(false);
                    }
                });
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
                await this.LoadThreadListAsync().ConfigureAwait(false);
            }
        }
    }
}
