// <copyright file="IAwfulErrorHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Awful.UI.Interfaces
{
    /// <summary>
    /// Awful Error Handler.
    /// </summary>
    public interface IAwfulErrorHandler
    {
        /// <summary>
        /// Handle error in UI.
        /// </summary>
        /// <param name="ex">Exception being thrown.</param>
        void HandleError(Exception ex);
    }
}
