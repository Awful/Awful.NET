// <copyright file="LepersPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Lepers View Model.
    /// </summary>
    public class LepersPageViewModel : AwfulViewModel
    {

        private BanActions banActions;
        private TemplateHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="LepersPageViewModel"/> class.
        /// </summary>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public LepersPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public WebView WebView { get; set; }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            if (this.IsSignedIn)
            {
                this.banActions = new BanActions(this.Client, this.Context, this.handler);
                await this.LoadLepersPage().ConfigureAwait(false);
            }
        }

        public async Task LoadLepersPage()
        {
            this.IsBusy = true;
            var defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            var banPage = await this.banActions.GetBanPageAsync().ConfigureAwait(false);
            var source = new HtmlWebViewSource();
            source.Html = this.banActions.RenderBanView(banPage, defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
            this.IsBusy = false;
        }
    }
}
