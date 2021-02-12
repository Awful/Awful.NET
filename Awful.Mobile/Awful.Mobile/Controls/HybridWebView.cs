﻿// <copyright file="HybridWebView.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.UI.Interfaces;
using Xamarin.Forms;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Hybrid Webview.
    /// Used to invoke Javascript onto a webpage.
    /// Originally posted at https://theconfuzedsourcecode.wordpress.com/2020/01/15/talking-to-your-webview-in-xamarin-forms/.
    /// </summary>
    public class HybridWebView : WebView, IAwfulWebview
    {
        private Action<string> action;

        /// <summary>
        /// Register Javascript Action.
        /// </summary>
        /// <param name="callback">Callback.</param>
        public void RegisterAction(Action<string> callback)
        {
            this.action = callback;
        }

        /// <summary>
        /// Cleanup Javascript Action.
        /// </summary>
        public void Cleanup()
        {
            this.action = null;
        }

        /// <summary>
        /// Invoke Action.
        /// </summary>
        /// <param name="data">Data used to invoke.</param>
        public void InvokeAction(string data)
        {
            if (this.action == null || data == null)
            {
                return;
            }

            this.action.Invoke(data);
        }

        /// <inheritdoc/>
        public void SetSource(string html)
        {
            var source = new HtmlWebViewSource();
            source.Html = html;
            Device.BeginInvokeOnMainThread(() => this.Source = source);
        }

        /// <inheritdoc/>
        Task IAwfulWebview.EvaluateJavaScriptAsync(string data)
        {
            return this.EvaluateJavaScriptAsync(data);
        }
    }
}
