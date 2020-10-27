// <copyright file="SigninViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Awful.Windows.Bookmarks.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;

namespace Awful.Windows.Bookmarks.ViewModels
{
    /// <summary>
    /// Signin View Model.
    /// </summary>
    public class SigninViewModel : AwfulViewModel
    {
        private string password = string.Empty;

        private string username = string.Empty;

        private SigninAction signin;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigninViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public SigninViewModel(IPlatformProperties properties, AwfulContext context)
            : base(context)
        {
            this.signin = new SigninAction(properties, context);
        }

        /// <summary>
        /// Gets a value indicating whether login is enabled.
        /// </summary>
        public bool IsLoginEnabled => !string.IsNullOrEmpty(this.Password) && !string.IsNullOrEmpty(this.Username);

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.SetProperty(ref this.password, value);
                this.OnPropertyChanged(nameof(this.IsLoginEnabled));
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return this.username;
            }

            set
            {
                this.SetProperty(ref this.username, value);
                this.OnPropertyChanged(nameof(this.IsLoginEnabled));
            }
        }

        /// <summary>
        /// Login User With Password.
        /// </summary>
        public async void LoginUserWithPassword()
        {
            if (!this.IsLoginEnabled)
            {
                return;
            }

            var result = await this.signin.SigninAsync(this.Username, this.password).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    var appFrame = Window.Current.Content as Frame;
                    if (appFrame != null)
                    {
                        appFrame.Navigate(typeof(BookmarksPage));
                    }
                });
            }
        }
    }
}
