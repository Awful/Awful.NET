// <copyright file="AwfulWebView.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.UI.Interfaces;
using Microsoft.UI.Xaml.Controls;

namespace Awful.Win.Controls
{
    /// <summary>
    /// Awful WebView.
    /// </summary>
    public class AwfulWebView : WebView2, IAwfulWebview
    {
        private const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";
        private Action<string> action;

        public AwfulWebView()
        {
            this.NavigationCompleted += this.AwfulWebView_NavigationCompleted;
            this.WebMessageReceived += this.AwfulWebView_WebMessageReceived;
        }

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
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => this.NavigateToString(html));
        }

        private void AwfulWebView_WebMessageReceived(WebView2 sender, WebView2WebMessageReceivedEventArgs args)
        {
            if (this.action != null)
            {
                this.action.Invoke(args.WebMessageAsString);
            }
        }

        private async void AwfulWebView_NavigationCompleted(WebView2 sender, WebView2NavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                // Inject JS script
                await this.ExecuteScriptAsync(JavaScriptFunction);
            }
        }
    }
}
