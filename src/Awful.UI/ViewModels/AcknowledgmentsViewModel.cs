// <copyright file="AcknowledgmentsViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Themes;
using Awful.UI.Controls;

namespace Awful.UI.ViewModels
{
    public class AcknowledgmentsViewModel : AwfulWebviewViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcknowledgmentsViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public AcknowledgmentsViewModel(IAwfulWebview webview, Action<string>? callback, IServiceProvider services)
            : base(webview, callback, services)
        {
        }

        /// <summary>
        /// Load Templates.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task LoadTemplateAsync()
        {
            IsBusy = true;
            var defaults = await GenerateDefaultOptionsAsync();
            WebView?.SetSource(Templates.RenderAcknowledgementstView(defaults));
            IsBusy = false;
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();
            await LoadTemplateAsync();
        }
    }
}
