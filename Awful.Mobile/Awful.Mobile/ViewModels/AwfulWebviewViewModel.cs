// <copyright file="AwfulWebviewViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Interfaces;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Awful Webview View Model.
    /// Used as the base for pages with Webviews.
    /// </summary>
    public class AwfulWebviewViewModel : AwfulViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulWebviewViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public AwfulWebviewViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; set; }

        /// <summary>
        /// Gets or sets the thread.
        /// </summary>
        protected AwfulThread Thread { get; set; }

        /// <summary>
        /// Sets the webview on the view model.
        /// </summary>
        /// <param name="webview">The Webview.</param>
        /// <param name="callback">The webview callback.</param>
        public void LoadWebview(IAwfulWebview webview, Action<string> callback = default)
        {
            if (webview == null)
            {
                throw new ArgumentNullException(nameof(webview));
            }

            this.WebView = webview;
            if (callback != null)
            {
                this.WebView.RegisterAction(callback);
            }
        }
    }
}
