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
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// User Profile Page View Model.
    /// </summary>
    public class UserProfilePageViewModel : AwfulViewModel
    {
        private UserActions userActions;
        private TemplateHandler handler;
        private long? profileId;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public UserProfilePageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
            this.Title = "User Profile";
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; set; }

        public void LoadProfile(long profileId, IAwfulWebview webview)
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
            this.WebView.SetSource(this.userActions.RenderProfileView(profile, defaults));
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
