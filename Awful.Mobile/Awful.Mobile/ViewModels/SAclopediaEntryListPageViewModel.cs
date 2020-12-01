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
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// SAclopedia Entry List View Model.
    /// On load, if signed in, load new SAclopedia items.
    /// </summary>
    public class SAclopediaEntryListPageViewModel : MobileAwfulViewModel
    {
        private SAclopediaAction saclopedia;
        private TemplateHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryListPageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful Template Handler.</param>
        /// <param name="context">Awful Context.</param>
        public SAclopediaEntryListPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public Command<SAclopediaEntryItem> SelectionCommand
        {
            get
            {
                return new Command<SAclopediaEntryItem>((item) =>
                {
                    if (item != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await PushDetailPageAsync(new SAclopediaEntryPage(item)).ConfigureAwait(false);
                        });
                    }
                });
            }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public Command RefreshCommand
        {
            get
            {
                return new Command(async () => await this.RefreshEntryList(true).ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Gets the SAclopedia Items.
        /// </summary>
        public List<SAclopediaGroup> Items { get; private set; } = new List<SAclopediaGroup>();

        public override async Task OnLoad()
        {
            if (this.IsSignedIn)
            {
                this.saclopedia = new SAclopediaAction(this.Client, this.Context, this.handler);
                await this.RefreshEntryList(false).ConfigureAwait(false);
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
            var items = await this.saclopedia.LoadSAclopediaEntryItemsAsync(refresh).ConfigureAwait(false);
            if (items.Any())
            {
                this.Items = items.GroupBy(n => n.Title[0].ToString().ToUpperInvariant()).Select(n => new SAclopediaGroup(n.Key, n.ToList())).OrderBy(n => n.Name).ToList();
                this.OnPropertyChanged(nameof(this.Items));
            }

            this.IsBusy = false;
        }
    }
}
