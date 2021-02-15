// <copyright file="SAclopediaEntryListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// SAclopedia Entry List View Model.
    /// On load, if signed in, load new SAclopedia items.
    /// </summary>
    public class SAclopediaEntryListPageViewModel : AwfulViewModel
    {
        private SAclopediaAction saclopedia;
        private ITemplateHandler handler;
        private AwfulAsyncCommand refreshCommand;
        private List<SAclopediaEntryItem> originalItems = new List<SAclopediaEntryItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Template Handler.</param>
        /// <param name="context">Awful Context.</param>
        public SAclopediaEntryListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<SAclopediaEntryItem> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<SAclopediaEntryItem>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            await this.Navigation.PushDetailPageAsync(new SAclopediaEntryPage(item)).ConfigureAwait(false);
                        }
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public AwfulAsyncCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new AwfulAsyncCommand(async () => await this.RefreshEntryList(true).ConfigureAwait(false), null, this.Error);
            }
        }

        /// <summary>
        /// Gets the SAclopedia Items.
        /// </summary>
        public List<SAclopediaGroup> Items { get; private set; } = new List<SAclopediaGroup>();

        /// <summary>
        /// Filter SAclopediaList, with text.
        /// </summary>
        /// <param name="text">Text to filter by.</param>
        public void FilterList(string text)
        {
            if (!this.originalItems.Any())
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                this.Items = this.originalItems.GroupBy(n => n.Title[0].ToString().ToUpperInvariant()).Select(n => new SAclopediaGroup(n.Key, n.ToList())).OrderBy(n => n.Name).ToList();
                this.OnPropertyChanged(nameof(this.Items));
                return;
            }

            var items = this.originalItems.Where(n => n.Title.Contains(text));
            this.Items = items.GroupBy(n => n.Title[0].ToString().ToUpperInvariant()).Select(n => new SAclopediaGroup(n.Key, n.ToList())).OrderBy(n => n.Name).ToList();
            this.OnPropertyChanged(nameof(this.Items));
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            if (this.IsSignedIn)
            {
                this.saclopedia = new SAclopediaAction(this.Client, this.Context, this.handler);
                if (!this.Items.Any())
                {
                    await this.RefreshEntryList(false).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Refreshes SAclopedia Items.
        /// </summary>
        /// <param name="refresh">Force refresh of cache.</param>
        /// <returns>A Task.</returns>
        public async Task RefreshEntryList(bool refresh)
        {
            this.IsBusy = true;
            this.originalItems = await this.saclopedia.LoadSAclopediaEntryItemsAsync(refresh).ConfigureAwait(false);
            if (this.originalItems.Any())
            {
                this.Items = this.originalItems.GroupBy(n => n.Title[0].ToString().ToUpperInvariant()).Select(n => new SAclopediaGroup(n.Key, n.ToList())).OrderBy(n => n.Name).ToList();
                this.OnPropertyChanged(nameof(this.Items));
            }

            this.IsBusy = false;
        }
    }
}
