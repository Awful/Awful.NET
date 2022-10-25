// <copyright file="LoginViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.UI.Actions;
using Awful.UI.Tools;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Login Page View Model.
    /// </summary>
    public class LoginViewModel : AwfulViewModel
    {
        private string password = string.Empty;

        private string username = string.Empty;

        private SigninAction signin;

        private AsyncCommand? loginCommand;

        public LoginViewModel(IServiceProvider services)
            : base(services)
        {
            signin = new SigninAction(PlatformServices, Context);
        }

        /// <summary>
        /// Gets a value indicating whether login is enabled.
        /// </summary>
        public bool IsLoginEnabled => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Username);

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public AsyncCommand LoginCommand
        {
            get
            {
                return loginCommand ??= new AsyncCommand(this.LoginUserWithPassword, () => IsLoginEnabled, Dispatcher, ErrorHandler);
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                SetProperty(ref password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                SetProperty(ref username, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Login User With Password.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoginUserWithPassword()
        {
            if (!IsLoginEnabled)
            {
                return;
            }

            IsBusy = true;
            var result = await signin.SigninAsync(Username, password).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                // await this.navigation.SetMainAppPageAsync().ConfigureAwait(false);
            }
            else
            {
                // await this.navigation.DisplayAlertAsync("Login Error", result.Error).ConfigureAwait(false);
            }

            IsBusy = false;
        }
    }
}
