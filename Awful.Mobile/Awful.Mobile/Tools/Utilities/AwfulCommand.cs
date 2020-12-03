// <copyright file="AwfulCommand.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Utilities
{
    /// <summary>
    /// Awful Command.
    /// </summary>
    public class AwfulCommand : ICommand
    {
        private readonly IAwfulErrorHandler errorHandler;
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> execute;
        private readonly WeakEventManager weakEventManager = new WeakEventManager();
        private bool isExecuting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulCommand"/> class.
        /// </summary>
        /// <param name="sender">Sender of command.</param>
        /// <param name="execute">Command to execute.</param>
        public AwfulCommand(Action<object> execute, IAwfulErrorHandler errorHandler = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.errorHandler = errorHandler;
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulCommand"/> class.
        /// </summary>
        /// <param name="execute">Command to execute.</param>
        public AwfulCommand(Action execute, IAwfulErrorHandler errorHandler = null)
            : this(o => execute(), errorHandler)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulCommand"/> class.
        /// </summary>
        /// <param name="execute">Command to execute.</param>
        /// <param name="canExecute">Can Execute Command.</param>
        public AwfulCommand(Action<object> execute, Func<object, bool> canExecute, IAwfulErrorHandler errorHandler = null)
            : this(execute, errorHandler)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            this.canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulCommand"/> class.
        /// </summary>
        /// <param name="execute">Command to execute.</param>
        /// <param name="canExecute">Can Execute Command.</param>
        public AwfulCommand(Action execute, Func<bool> canExecute, IAwfulErrorHandler errorHandler = null)
            : this(o => execute(), o => canExecute(), errorHandler)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add { this.weakEventManager.AddEventHandler(value); }
            remove { this.weakEventManager.RemoveEventHandler(value); }
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute != null)
            {
                return !this.isExecuting && this.canExecute(parameter);
            }

            return !this.isExecuting;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if(this.CanExecute(parameter))
            {
                try
                {
                    this.isExecuting = true;
                    this.RaiseCanExecuteChanged();
                    this.execute(parameter);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    if (this.errorHandler != null)
                    {
                        this.errorHandler.HandleError(ex);
                    }
                }
            }

            this.isExecuting = false;
            this.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Change Can Execute.
        /// </summary>
#pragma warning disable CA1030 // Use events where appropriate
        public void RaiseCanExecuteChanged()
#pragma warning restore CA1030 // Use events where appropriate
        {
            this.weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(this.CanExecuteChanged));
        }
    }
}
