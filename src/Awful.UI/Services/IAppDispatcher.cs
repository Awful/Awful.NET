// <copyright file="IAppDispatcher.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.UI.Services
{
    /// <summary>
    /// App Dispatcher.
    /// </summary>
    public interface IAppDispatcher
    {
        /// <summary>
        /// Dispatch.
        /// </summary>
        /// <param name="action">Action to Dispatch.</param>
        /// <returns>Boolean.</returns>
        bool Dispatch(Action action);
    }
}
