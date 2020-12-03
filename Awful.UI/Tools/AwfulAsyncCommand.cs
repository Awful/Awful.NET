// <copyright file="AwfulAsyncCommand.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.UI.Interfaces;
using Xamarin.Essentials;

namespace Awful.UI.Tools
{
    /// <summary>
    /// Awful Async Command.
    /// </summary>
    public class AwfulAsyncCommand : IAwfulAsyncCommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;
        private readonly IAwfulErrorHandler errorHandler;
        private bool isExecuting;

        protected bool IsExecuting
        {
            get
            {
                return this.isExecuting;
            }

            set
            {
                this.isExecuting = value;
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">Command to execute.</param>
        /// <param name="canExecute">Can execute command.</param>
        /// <param name="errorHandler">Error handler.</param>
        public AwfulAsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null,
            IAwfulErrorHandler errorHandler = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        /// <summary>
        /// Can Execute Changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute()
        {
            return !this.IsExecuting && (this.canExecute?.Invoke() ?? true);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync()
        {
            if (this.CanExecute())
            {
                try
                {
                    this.IsExecuting = true;
                    await this.execute().ConfigureAwait(false);
                }
                finally
                {
                    this.IsExecuting = false;
                }
            }
        }

        /// <summary>
        /// Raises Can Execute Changed.
        /// </summary>
#pragma warning disable CA1030 // Use events where appropriate
        public void RaiseCanExecuteChanged()
#pragma warning restore CA1030 // Use events where appropriate
        {
            MainThread.BeginInvokeOnMainThread(() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        /// <inheritdoc/>
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        /// <inheritdoc/>
        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync().FireAndForgetSafeAsync(this.errorHandler);
        }
    }
}
