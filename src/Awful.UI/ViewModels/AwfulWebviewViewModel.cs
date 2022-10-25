// <copyright file="AwfulWebviewViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.UI.Controls;
using Awful.UI.Entities;

namespace Awful.UI.ViewModels
{
    public class AwfulWebviewViewModel : AwfulViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulWebviewViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public AwfulWebviewViewModel(IAwfulWebview webview, Action<string>? callback, IServiceProvider services)
            : base(services)
        {
            if (webview == null)
            {
                throw new ArgumentNullException(nameof(webview));
            }

            WebView = webview;

            if (callback != null)
            {
                WebView.RegisterAction(callback);
            }
        }

        /// <summary>
        /// Gets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; }

        /// <summary>
        /// Gets or sets the thread.
        /// </summary>
        protected AwfulThread? Thread { get; set; }
    }
}
