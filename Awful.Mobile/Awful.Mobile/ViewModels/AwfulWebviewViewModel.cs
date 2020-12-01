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
        /// Sets the webview on the view model.
        /// Also sets up the Javascript actions to handle commands
        /// from the webview, if given.
        /// </summary>
        /// <param name="webview">The Webview.</param>
        /// <param name="javascriptAction">Javascript Action to handle commands from the webview.</param>
        public void LoadWebview(HybridWebView webview)
        {
            if (webview == null)
            {
                throw new ArgumentNullException(nameof(webview));
            }

            this.WebView = webview;
            this.WebView.RegisterAction(this.HandleDataFromJavascript);
        }

        private void HandleDataFromJavascript(string data)
        {
            var json = JsonConvert.DeserializeObject<WebViewDataInterop>(data);
            switch (json.Type)
            {
                case "showPostMenu":
                    Device.BeginInvokeOnMainThread(async () => {

                        var result = await App.Current.MainPage.DisplayActionSheet("Post Options", "Cancel", null, "Share", "Mark Read", "Quote Post").ConfigureAwait(false);
                        switch (result)
                        {
                            case "Share":
                                await Share.RequestAsync(new ShareTextRequest
                                {
                                    Uri = string.Format(CultureInfo.InvariantCulture, EndPoints.ShowPost, json.Id),
                                    Title = this.Title,
                                }).ConfigureAwait(false);
                                break;
                            case "Mark Read":
                                //Task.Run(() => this.MarkPostAsUnreadAsync(json.Id));
                                break;
                            case "Quote Post":
                                //await PushModalAsync(new ThreadReplyPage(this.thread.ThreadId, json.Id, false)).ConfigureAwait(false);
                                break;
                        }
                    });
                    break;
            }
        }
    }
}
