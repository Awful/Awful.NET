// <copyright file="UserProfilePageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.Threads;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class UserProfilePageViewModel : MobileAwfulViewModel
    {
        private UserActions userActions;
        private TemplateHandler handler;
        private long? profileId;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public UserProfilePageViewModel(TemplateHandler handler, AwfulContext context)
            : base(context)
        {
            this.handler = handler;
            this.Title = "User Profile";
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public HybridWebView WebView { get; set; }

        public void LoadProfile(long profileId, HybridWebView webview)
        {
            this.profileId = profileId;
            this.WebView = webview;
        }

        public async Task LoadTemplate(long profileId)
        {
            this.IsBusy = true;
            var defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            var profile = await this.userActions.GetUserFromProfilePageAsync(profileId).ConfigureAwait(false);
            this.Title = profile.Username;
            var source = new HtmlWebViewSource();
            source.Html = this.userActions.RenderProfileView(profile, defaults);
            Device.BeginInvokeOnMainThread(() => this.WebView.Source = source);
            this.IsBusy = false;
        }

        public override async Task OnLoad()
        {
            this.userActions = new UserActions(this.Client, this.Context, this.handler);
            if (this.profileId != null)
            {
                await this.LoadTemplate(this.profileId.Value).ConfigureAwait(false);
            }
        }
    }
}
