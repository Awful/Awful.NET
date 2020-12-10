// <copyright file="LoginPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Login Page View Model.
    /// </summary>
    public class LoginPageViewModel : AwfulViewModel
    {
        private string password = string.Empty;

        private string username = string.Empty;

        private SigninAction signin;

        private AwfulAsyncCommand loginCommand;

        private IAwfulNavigation navigation;

        private IAwfulErrorHandler error;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPageViewModel"/> class.
        /// </summary>
        /// <param name="error">Awful Error Handler.</param>
        /// <param name="navigation">Awful Navigation.</param>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public LoginPageViewModel(
            IAwfulErrorHandler error,
            IAwfulNavigation navigation,
            IPlatformProperties properties,
            AwfulContext context)
            : base(context)
        {
            this.error = error;
            this.navigation = navigation;
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
                return this.loginCommand ??= new AwfulAsyncCommand(this.LoginUserWithPassword, () => this.IsLoginEnabled, this.error);
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
                await this.navigation.SetMainAppPageAsync().ConfigureAwait(false);
            }
            else
            {
                await this.navigation.DisplayAlertAsync("Login Error", result.Error).ConfigureAwait(false);
            }

            this.IsBusy = false;
        }
    }
}
