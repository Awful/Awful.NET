// <copyright file="AwfulAsyncCommandGeneric.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.UI.Interfaces;
using Awful.UI.Tools;

namespace Awful.Mobile.Tools.Utilities
{
    public class AwfulAsyncCommand<T> : IAwfulAsyncCommand<T>
    {
        private readonly Func<T, Task> execute;
        private readonly Func<T, bool> canExecute;
        private readonly IAwfulErrorHandler errorHandler;
        private bool isExecuting;

        public event EventHandler CanExecuteChanged;

        public AwfulAsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, IAwfulErrorHandler errorHandler = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        public bool CanExecute(T parameter)
        {
            return !this.isExecuting && (this.canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (this.CanExecute(parameter))
            {
                try
                {
                    this.isExecuting = true;
                    await this.execute(parameter).ConfigureAwait(false);
                }
                finally
                {
                    this.isExecuting = false;
                }
            }

            this.RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync((T)parameter).FireAndForgetSafeAsync(this.errorHandler);
        }
    }
}
