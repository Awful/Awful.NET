// <copyright file="JsBridge.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Android.Webkit;
using Awful.Mobile.Controls;
using Java.Interop;

namespace Awful.Mobile.Droid.Renderers
{
    /// <summary>
    /// Javascript Bridge.
    /// </summary>
    public class JsBridge : Java.Lang.Object
    {
        private readonly WeakReference<HybridWebViewRenderer> hybridWebViewMainRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsBridge"/> class.
        /// </summary>
        /// <param name="hybridRenderer">Hybrid Webview Renderer.</param>
        public JsBridge(HybridWebViewRenderer hybridRenderer)
        {
            this.hybridWebViewMainRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
        }

        /// <summary>
        /// Invoke Action.
        /// </summary>
        /// <param name="data">Data to Invoke.</param>
        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            if (this.hybridWebViewMainRenderer != null && this.hybridWebViewMainRenderer.TryGetTarget(out var hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).InvokeAction(data);
            }
        }
    }
}
