﻿// <copyright file="IErrorHandlerService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.UI.Services
{
    /// <summary>
    /// Error Handler Service.
    /// </summary>
    public interface IErrorHandlerService
    {
        /// <summary>
        /// Handle error in UI.
        /// </summary>
        /// <param name="ex">Exception being thrown.</param>
        void HandleError(Exception ex);
    }
}
