// <copyright file="AcknowledgmentsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
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
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public AcknowledgmentsPageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
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
            var source = new HtmlWebViewSource();
            source.Html = this.handler.RenderAcknowledgementstView(this.defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await this.LoadTemplateAsync().ConfigureAwait(false);
        }
    }
}
