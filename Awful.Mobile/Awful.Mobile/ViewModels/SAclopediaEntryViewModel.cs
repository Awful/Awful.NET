// <copyright file="SAclopediaEntryViewModel.cs" company="Drastic Actions">
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
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// SAclopedia Entry View Model.
    /// </summary>
    public class SAclopediaEntryViewModel : AwfulViewModel
    {
        private SAclopediaAction saclopedia;
        private TemplateHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful Template Handler.</param>
        /// <param name="context">Awful Context.</param>
        public SAclopediaEntryViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public WebView WebView { get; set; }

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

            this.IsRefreshing = true;
            this.Title = entryListItem.Title;
            var defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            var entry = await this.saclopedia.LoadSAclopediaEntryAsync(entryListItem).ConfigureAwait(false);
            var source = new HtmlWebViewSource();
            source.Html = this.saclopedia.GenerateSAclopediaEntryTemplate(entry, defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
            await Task.Delay(2000).ConfigureAwait(false);
            this.IsRefreshing = false;
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            this.saclopedia = new SAclopediaAction(this.Client, this.Context, this.handler);
            return base.OnLoad();
        }
    }
}
