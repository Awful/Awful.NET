// <copyright file="IAwfulErrorHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Awful.UI.Interfaces
{
    public interface IAwfulErrorHandler
    {
        void HandleError(Exception ex);
    }
}
