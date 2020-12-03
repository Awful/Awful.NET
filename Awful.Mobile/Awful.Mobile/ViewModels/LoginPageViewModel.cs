// <copyright file="LoginPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Mobile.Tools.Utilities;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Login Page View Model.
    /// </summary>
    public class LoginPageViewModel : MobileAwfulViewModel
    {
        private string password = string.Empty;

        private string username = string.Empty;

        private SigninAction signin;

        private AwfulAsyncCommand loginCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPageViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public LoginPageViewModel(IPlatformProperties properties, AwfulContext context)
            : base(context)
        {
            this.signin = new SigninAction(properties, context);
        }

        /// <summary>
        /// Gets a value indicating whether login is enabled.
        /// </summary>
        public bool IsLoginEnabled => !string.IsNullOrEmpty(this.Password) && !string.IsNullOrEmpty(this.Username);

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public AwfulAsyncCommand LoginCommand
        {
            get
            {
                return this.loginCommand ??= new AwfulAsyncCommand(this.LoginUserWithPassword, () => this.IsLoginEnabled, this);
            }
        }

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
                this.LoginCommand.RaiseCanExecuteChanged();
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
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Login User With Password.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoginUserWithPassword()
        {
            if (!this.IsLoginEnabled)
            {
                return;
            }

            this.IsBusy = true;
            var result = await this.signin.SigninAsync(this.Username, this.password).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                await SetMainAppPageAsync().ConfigureAwait(false);
            }
            else
            {
                await DisplayAlertAsync("Login Error", result.Error).ConfigureAwait(false);
            }

            this.IsBusy = false;
        }
    }
}
