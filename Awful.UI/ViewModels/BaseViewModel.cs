// <copyright file="BaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Awful.Database;
using Awful.Database.Entities;
using Awful.UI.Interfaces;
using Xamarin.Essentials;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Base View Model.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string title = string.Empty;
        private bool isBusy = false;
        private UserAuth currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="error">Error Handler.</param>
        /// <param name="navigation">Navigation Handler.</param>
        public BaseViewModel(IDatabase database, IAwfulErrorHandler error, IAwfulNavigationHandler navigation)
        {
            this.Database = database;
            this.Error = error;
            this.Navigation = navigation;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when CanExecute is changed.
        /// </summary>
        public event EventHandler RaiseCanExecuteChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the view is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.SetProperty(ref this.isBusy, value);
                this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public UserAuth CurrentUser
        {
            get
            {
                return this.currentUser;
            }

            set
            {
                this.SetProperty(ref this.currentUser, value);
                this.OnPropertyChanged(nameof(this.IsUserLoggedIn));
                this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is logged in.
        /// </summary>
        public bool IsUserLoggedIn => this.CurrentUser != null;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        /// <summary>
        /// Gets the database instance.
        /// </summary>
        protected IDatabase Database { get; private set; }

        /// <summary>
        /// Gets the navigation handler.
        /// </summary>
        protected IAwfulNavigationHandler Navigation { get; private set; }

        /// <summary>
        /// Gets the error handler.
        /// </summary>
        protected IAwfulErrorHandler Error { get; private set; }

        /// <summary>
        /// Called when the page is appearing.
        /// </summary>
        /// <returns>Task.</returns>
        public virtual Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }

#pragma warning disable SA1600 // Elements should be documented
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
#pragma warning restore SA1600 // Elements should be documented
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var changed = this.PropertyChanged;
                if (changed == null)
                {
                    return;
                }

                changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }
    }
}
