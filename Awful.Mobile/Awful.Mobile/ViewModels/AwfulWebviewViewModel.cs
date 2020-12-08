// <copyright file="AwfulWebviewViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Threading.Tasks;
using Autofac;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Awful Webview View Model.
    /// Used as the base for pages with Webviews.
    /// </summary>
    public class AwfulWebviewViewModel : MobileAwfulViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulWebviewViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public AwfulWebviewViewModel(AwfulContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public HybridWebView WebView { get; set; }

        /// <summary>
        /// Gets or sets the thread.
        /// </summary>
        protected AwfulThread Thread { get; set; }

        /// <summary>
        /// Sets the webview on the view model.
        /// </summary>
        /// <param name="webview">The Webview.</param>
        /// <param name="callback">The webview callback.</param>
        public void LoadWebview(HybridWebView webview, Action<string> callback = default)
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
