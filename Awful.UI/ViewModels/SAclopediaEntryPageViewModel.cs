// <copyright file="SAclopediaEntryPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.SAclopedia;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// SAclopedia Entry View Model.
    /// </summary>
    public class SAclopediaEntryPageViewModel : AwfulViewModel
    {
        private SAclopediaAction saclopedia;
        private ITemplateHandler handler;
        private SAclopediaEntryItem entry;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Template Handler.</param>
        /// <param name="context">Awful Context.</param>
        public SAclopediaEntryPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; set; }

        public void LoadEntry(SAclopediaEntryItem entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            this.entry = entry;
            this.Title = entry.Title;
        }

        /// <summary>
        /// Loads SAclopedia Template into webview.
        /// </summary>
        /// <param name="entryListItem">SAclopedia Entry List Item.</param>
        /// <returns>Task.</returns>
        public async Task LoadTemplate(SAclopediaEntryItem entryListItem)
        {
            if (entryListItem == null)
            {
                throw new ArgumentNullException(nameof(entryListItem));
            }

            this.IsBusy = true;
            this.Title = entryListItem.Title;
            var defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            var entry = await this.saclopedia.LoadSAclopediaEntryAsync(entryListItem).ConfigureAwait(false);
            this.WebView.SetSource(this.saclopedia.GenerateSAclopediaEntryTemplate(entry, defaults));
            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.saclopedia = new SAclopediaAction(this.Client, this.Context, this.handler);
            if (this.entry != null)
            {
                await this.LoadTemplate(this.entry).ConfigureAwait(false);
            }
        }
    }
}
