// <copyright file="AcknowledgmentsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Acknowledgments Page View Model.
    /// </summary>
    public class AcknowledgmentsPageViewModel : AwfulWebviewViewModel
    {
        private TemplateHandler handler;
        private DefaultOptions defaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcknowledgmentsPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public AcknowledgmentsPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Load Templates.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task LoadTemplateAsync()
        {
            this.IsBusy = true;
            this.defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            this.WebView.SetSource(this.handler.RenderAcknowledgementstView(this.defaults));
            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await this.LoadTemplateAsync().ConfigureAwait(false);
        }
    }
}
