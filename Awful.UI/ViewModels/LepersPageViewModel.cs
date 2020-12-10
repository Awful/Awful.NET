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
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.UI.ViewModels
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
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public LepersPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, AwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; set; }

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
            this.WebView.SetSource(this.banActions.RenderBanView(banPage, defaults));
            this.IsBusy = false;
        }
    }
}
