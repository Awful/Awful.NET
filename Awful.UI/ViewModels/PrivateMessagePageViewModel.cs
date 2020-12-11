// <copyright file="PrivateMessagePageViewModel.cs" company="Drastic Actions">
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
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Private Message Page View Model.
    /// </summary>
    public class PrivateMessagePageViewModel : AwfulViewModel
    {
        protected AwfulPM pm;
        private PrivateMessageActions pmActions;
        private DefaultOptions defaults;
        private ITemplateHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagePageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Template Handler.</param>
        /// <param name="context">Awful Context.</param>
        public PrivateMessagePageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; set; }

        /// <summary>
        /// Load PM.
        /// </summary>
        /// <param name="pm">PM.</param>
        public void LoadPM(AwfulPM pm)
        {
            if (pm == null)
            {
                throw new ArgumentNullException(nameof(pm));
            }

            this.pm = pm;
            this.Title = this.pm.Title;
        }

        /// <summary>
        /// Loads PM Template into webview.
        /// </summary>
        /// <param name="pmId">Private Message Id.</param>
        /// <returns>Task.</returns>
        public async Task LoadTemplate(int pmId)
        {
            this.IsBusy = true;

            this.defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            var post = await this.pmActions.GetPrivateMessageAsync(pmId).ConfigureAwait(false);
            this.WebView.SetSource(this.pmActions.RenderPrivateMessageView(post, this.defaults));
            await Task.Delay(2000).ConfigureAwait(false);

            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.pmActions = new PrivateMessageActions(this.Client, this.Context, this.handler);
            if (this.pm != null)
            {
                await this.LoadTemplate(this.pm.PrivateMessageId).ConfigureAwait(false);
            }
        }
    }
}
