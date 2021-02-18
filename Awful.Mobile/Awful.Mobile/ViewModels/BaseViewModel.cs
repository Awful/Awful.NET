﻿// <copyright file="BaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Base View Model.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private LayoutState currentState = LayoutState.Loading;
        private bool isRefreshing = false;
        private string title = string.Empty;
        private string loadingText = "Loading...";

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the view is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.currentState == LayoutState.Loading;
            }

            set
            {
                this.CurrentState = value ? LayoutState.Loading : LayoutState.None;
            }
        }

        /// <summary>
        /// Gets or sets the current layout state.
        /// </summary>
        public LayoutState CurrentState
        {
            get
            {
                return this.currentState;
            }

            set
            {
                this.SetProperty(ref this.currentState, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }

            set
            {
                this.SetProperty(ref this.isRefreshing, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        /// <summary>
        /// Gets or sets the loading text.
        /// </summary>
        public string LoadingText
        {
            get { return this.loadingText; }
            set { this.SetProperty(ref this.loadingText, value); }
        }

        /// <summary>
        /// Called when wanting to raise a Command Can Execute.
        /// </summary>
        public virtual void RaiseCanExecuteChanged()
        {
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
