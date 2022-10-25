// <copyright file="AwfulViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Themes;
using Awful.UI.Entities;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Awful View Model.
    /// </summary>
    public class AwfulViewModel : BaseViewModel, IDisposable
    {
        private bool onProbation;
        private string onProbationText = string.Empty;
        private UserAuth? user;
        private bool disposed;
        private bool canPM;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public AwfulViewModel(IServiceProvider services)
            : base(services)
        {
            Client = new AwfulClient();
        }

        /// <summary>
        /// Gets the Awful Client.
        /// </summary>
        public AwfulClient Client { get; private set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public UserAuth? CurrentUser
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn
        {
            get { return user != null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is on probation.
        /// </summary>
        public bool OnProbation
        {
            get
            {
                return onProbation;
            }

            set
            {
                SetProperty(ref onProbation, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can private message.
        /// </summary>
        public bool CanPM
        {
            get
            {
                return canPM;
            }

            set
            {
                SetProperty(ref canPM, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the inner probation error text.
        /// </summary>
        public string OnProbationText
        {
            get { return onProbationText; }
            set { SetProperty(ref onProbationText, value); }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            if (Context != null)
            {
                user = await Context.GetDefaultUserAsync().ConfigureAwait(false);
                CanPM = user != null && user.RecievePM;
                OnPropertyChanged(nameof(IsSignedIn));
                Client = new AwfulClient(user != null ? user.AuthCookies : new System.Net.CookieContainer());
            }
        }

        /// <summary>
        /// Generates the default options for Awful.NET.
        /// </summary>
        /// <returns><see cref="DefaultOptions"/>.</returns>
        public async Task<DefaultOptions> GenerateDefaultOptionsAsync()
        {
            var defaults = Context.GetAppSettings();
            var defaultOptions = new DefaultOptions() { IsDarkMode = defaults.UseDarkMode, IsOledMode = defaults.CustomTheme == AppCustomTheme.OLED };

            return defaultOptions;
        }

        /// <summary>
        /// Disposing.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed.
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    Context.Dispose();
                    Client?.Dispose();
                }
            }

            disposed = true;
        }
    }
}
