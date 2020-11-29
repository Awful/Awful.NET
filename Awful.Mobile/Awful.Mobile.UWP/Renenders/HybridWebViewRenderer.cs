// <copyright file="HybridWebViewRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Mobile.Controls;
using Awful.Mobile.UWP.Renenders;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Awful.Mobile.UWP.Renenders
{
    /// <summary>
    /// Hybrid Web View Renderer.
    /// </summary>
    public class HybridWebViewRenderer : WebViewRenderer
    {
        private const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";

        /// <inheritdoc/>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if (this.Control == null)
                {
                    return;
                }

                this.Control.NavigationCompleted -= this.OnWebViewNavigationCompleted;
                this.Control.ScriptNotify -= this.OnWebViewScriptNotify;
            }

            if (e.NewElement != null)
            {
                this.Control.NavigationCompleted += this.OnWebViewNavigationCompleted;
                this.Control.ScriptNotify += this.OnWebViewScriptNotify;
            }
        }

        private async void OnWebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                // Inject JS script
                await this.Control.InvokeScriptAsync("eval", new[] { JavaScriptFunction });
            }
        }

        private void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            ((HybridWebView)this.Element).InvokeAction(e.Value);
        }
    }
}
