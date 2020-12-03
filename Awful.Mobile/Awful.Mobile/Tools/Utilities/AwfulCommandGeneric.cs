// <copyright file="AwfulCommandGeneric.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Windows.Input;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Utilities
{
    /// <summary>
    /// Awful Command.
    /// </summary>
    /// <typeparam name="T">Generic Parameter.</typeparam>
#pragma warning disable SA1649 // File name should match first type name
    public sealed class AwfulCommand<T> : Command
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulCommand{T}"/> class.
        /// </summary>
        /// <param name="sender">Sender of command.</param>
        /// <param name="execute">Command to execute.</param>
        public AwfulCommand(Action<T> execute, IAwfulErrorHandler errorHandler = null)
            : base(o =>
            {
                if (IsValidParameter(o))
                {
                    try
                    {
                        execute((T)o);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        if (errorHandler != null)
                        {
                            errorHandler.HandleError(ex);
                        }
                    }
                }
            })
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulCommand{T}"/> class.
        /// </summary>
        /// <param name="sender">Sender of command.</param>
        /// <param name="execute">Command to execute.</param>
        /// <param name="canExecute">Can Execute Command.</param>
        public AwfulCommand(Action<T> execute, Func<T, bool> canExecute, IAwfulErrorHandler errorHandler = null)
            : base(
                o =>
            {
                if (IsValidParameter(o))
                {
                    try
                    {
                        execute((T)o);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        if (errorHandler != null)
                        {
                            errorHandler.HandleError(ex);
                        }
                    }
                }
            }, o => IsValidParameter(o) && canExecute((T)o))
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

        private static bool IsValidParameter(object o)
        {
            if (o != null)
            {
                // The parameter isn't null, so we don't have to worry whether null is a valid option
                return o is T;
            }

            var t = typeof(T);

            // The parameter is null. Is T Nullable?
            if (Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }

            // Not a Nullable, if it's a value type then null is not valid
            return !t.GetTypeInfo().IsValueType;
        }
    }
}
