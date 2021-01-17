// <copyright file="HybridWebViewRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.Content;
using Awful.Mobile.Controls;
using Awful.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Awful.Mobile.Droid.Renderers
{
    /// <summary>
    /// Hybrid WebView Renderer.
    /// </summary>
    public class HybridWebViewRenderer : WebViewRenderer
    {
        private const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        private readonly Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebViewRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public HybridWebViewRenderer(Context context)
            : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);

            if (e != null && e.OldElement != null)
            {
                this.Control.RemoveJavascriptInterface("jsBridge");
                ((HybridWebView)this.Element).Cleanup();
            }

            if (e != null && e.NewElement != null)
            {
                this.Control.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavascriptFunction}"));
                this.Control.AddJavascriptInterface(new JsBridge(this), "jsBridge");
            }
        }
    }
}
