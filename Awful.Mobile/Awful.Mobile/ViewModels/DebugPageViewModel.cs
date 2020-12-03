// <copyright file="DebugPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Threading.Tasks;
using Autofac;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.Mobile.Tools.Utilities;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Debug Page View Model.
    /// </summary>
    public class DebugPageViewModel : MobileAwfulViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public DebugPageViewModel(AwfulContext context)
            : base(context)
        {
            this.ThrowAsyncExceptionCommand = new AwfulAsyncCommand(this.ThrowAsyncDebugException, this.CanExecuteButtonPress, this);
        }

        /// <summary>,
        /// Gets the throw exception command.
        /// </summary>
        public AwfulCommand ThrowExceptionCommand
        {
            get
            {
                return new AwfulCommand(this.ThrowDebugException, this.CanExecuteButtonPress, this);
            }
        }

        private bool CanExecuteButtonPress()
        {
            return !this.IsBusy;
        }

        /// <summary>
        /// Gets the throw exception command.
        /// </summary>
        public AwfulAsyncCommand ThrowAsyncExceptionCommand { get; internal set; }

        private async Task ThrowAsyncDebugException()
        {
            this.IsBusy = true;
            //this.OnPropertyChanged("ThrowAsyncExceptionCommand");
            await Task.Delay(2000).ConfigureAwait(false);
            throw new Exception("OH NO!");
        }

        private void ThrowDebugException()
        {
            this.IsBusy = true;
            throw new Exception("OH NO!");
        }
    }
}
