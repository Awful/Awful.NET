// <copyright file="AsyncCommand.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Windows.Input;
using Awful.UI.Services;

namespace Awful.UI.Tools
{
    /// <summary>
    /// Async Command.
    /// </summary>
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task>? execute;
        private readonly Func<bool>? canExecute;
        private readonly IErrorHandlerService? errorHandler;
        private readonly IAppDispatcher? dispatcher;
        private bool isExecuting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">Command to execute.</param>
        /// <param name="canExecute">Can execute command.</param>
        /// <param name="dispatcher">Dispatcher.</param>
        /// <param name="errorHandler">Error handler.</param>
        public AsyncCommand(
            Func<Task> execute,
            Func<bool>? canExecute = null,
            IAppDispatcher? dispatcher = null,
            IErrorHandlerService? errorHandler = null)
        {
            this.dispatcher = dispatcher;
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        /// <summary>
        /// Can Execute Changed.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the command is executing.
        /// </summary>
        protected bool IsExecuting
        {
            get
            {
                return isExecuting;
            }

            set
            {
                isExecuting = value;
                RaiseCanExecuteChanged();
            }
        }

        /// <inheritdoc/>
        public bool CanExecute()
        {
            return !IsExecuting && (canExecute?.Invoke() ?? true);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                if (execute is not null)
                {
                    try
                    {
                        IsExecuting = true;
                        await execute().ConfigureAwait(false);
                    }
                    finally
                    {
                        IsExecuting = false;
                    }
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
            dispatcher?.Dispatch(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        /// <inheritdoc/>
        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute();
        }

        /// <inheritdoc/>
        void ICommand.Execute(object? parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }
    }
}
