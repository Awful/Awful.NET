// <copyright file="JavascriptWebViewClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Android.Webkit;

namespace Awful.Mobile.Droid.Renderers
{
    /// <summary>
    /// Javascript WebView Client.
    /// </summary>
    public class JavascriptWebViewClient : WebViewClient
    {
        private readonly string javascript;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavascriptWebViewClient"/> class.
        /// </summary>
        /// <param name="javascript">Javascript to invoke.</param>
        public JavascriptWebViewClient(string javascript)
        {
            this.javascript = javascript;
        }

        /// <inheritdoc/>
        public override void OnPageFinished(WebView view, string url)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            base.OnPageFinished(view, url);
            view.EvaluateJavascript(this.javascript, null);
        }
    }
}
