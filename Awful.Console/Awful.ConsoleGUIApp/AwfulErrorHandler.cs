// <copyright file="AwfulErrorHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;
using Awful.UI.Interfaces;

namespace Awful.ConsoleGUIApp
{
    /// <summary>
    /// Awful Error Handler.
    /// </summary>
    public class AwfulErrorHandler : IAwfulErrorHandler
    {
        /// <inheritdoc/>
        public void HandleError(Exception exception)
        {
        }
    }
}
