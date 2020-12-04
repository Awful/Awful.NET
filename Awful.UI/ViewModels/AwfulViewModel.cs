// <copyright file="AwfulViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Webview.Entities.Themes;
using Xamarin.Forms;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Awful View Model.
    /// </summary>
    public class AwfulViewModel : BaseViewModel, IDisposable
    {
        private bool onProbation;
        private string onProbationText;
        private UserAuth user;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Database Context.</param>
        public AwfulViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Database Context.</param>
        public AwfulViewModel(AwfulContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.Context = context;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is on probation.
        /// </summary>
        public bool OnProbation
        {
            get
            {
                return this.onProbation;
            }

            set
            {
                this.SetProperty(ref this.onProbation, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the inner probation error text.
        /// </summary>
        public string OnProbationText
        {
            get { return this.onProbationText; }
            set { this.SetProperty(ref this.onProbationText, value); }
        }

        /// <summary>
        /// Gets or sets the Awful Client.
        /// </summary>
        public AwfulClient Client { get; set; }

        /// <summary>
        /// Gets or sets the awful Database Context.
        /// </summary>
        public AwfulContext Context { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public UserAuth CurrentUser
        {
            get { return this.user; }
            set { this.SetProperty(ref this.user, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn
        {
            get { return this.user != null; }
        }

        /// <summary>
        /// Called when modal closes.
        /// </summary>
        public virtual void OnCloseModal()
        {
            this.OnPropertyChanged();
        }

        /// <summary>
        /// Setup VM on load.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SetupVM()
        {
            if (this.Context != null)
            {
                this.user = await this.Context.GetDefaultUserAsync().ConfigureAwait(false);
                this.OnPropertyChanged(nameof(this.IsSignedIn));
                this.Client = new AwfulClient(this.user != null ? this.user.AuthCookies : new System.Net.CookieContainer());
            }

            await this.OnLoad().ConfigureAwait(false);
        }

        /// <summary>
        /// Called on VM Load.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual async Task OnLoad()
        {
        }

        /// <summary>
        /// Generates the default options for Awful.NET.
        /// </summary>
        /// <returns><see cref="DefaultOptions"/>.</returns>
        public async Task<DefaultOptions> GenerateDefaultOptionsAsync()
        {
            var defaults = await this.Context.GetDefaultSettingsAsync().ConfigureAwait(false);
            var defaultOptions = new DefaultOptions() { DeviceColorTheme = defaults.DeviceColorTheme, DeviceTheme = defaults.DeviceTheme };
            if (defaultOptions.DeviceTheme == DeviceTheme.Default)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        defaultOptions.DeviceTheme = DeviceTheme.iOS;
                        break;
                    case Device.Android:
                        defaultOptions.DeviceTheme = DeviceTheme.Android;
                        break;
                }
            }

            return defaultOptions;
        }

        /// <summary>
        /// Disposing.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed.
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.Context.Dispose();
                    this.Client.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
