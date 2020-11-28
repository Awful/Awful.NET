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
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class PrivateMessagePageViewModel : AwfulViewModel
    {
        PrivateMessageActions pmActions;
        private Command refreshCommand;
        private AwfulPM pm;
        private DefaultOptions defaults;
        private TemplateHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessagePageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public PrivateMessagePageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public WebView WebView { get; set; }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        public Command RefreshCommand
        {
            get
            {
                return this.refreshCommand ??= new Command(async () =>
                {
                    if (this.pm != null)
                    {
                        this.IsRefreshing = true;
                        await this.LoadTemplate(this.pm.PrivateMessageId).ConfigureAwait(false);
                        this.IsRefreshing = false;
                    }
                });
            }
        }

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
        /// <returns>Task.</returns>
        public async Task LoadTemplate(int pmId)
        {
            this.IsBusy = true;

            this.defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            var post = await this.pmActions.GetPrivateMessageAsync(pmId).ConfigureAwait(false);
            var source = new HtmlWebViewSource();
            source.Html = this.pmActions.RenderPrivateMessageView(post, this.defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
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
