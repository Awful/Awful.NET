// <copyright file="AwfulErrorHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.UI.Interfaces;

namespace Awful.Mobile.Tools
{
    public class AwfulErrorHandler : IAwfulErrorHandler
    {
        private IAwfulNavigationHandler navigation;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulErrorHandler"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation.</param>
        public AwfulErrorHandler(IAwfulNavigationHandler navigation)
        {
            this.navigation = navigation;
        }

        /// <inheritdoc/>
        public void HandleError(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            string errorMessage = $"An {exception.GetType().FullName} was thrown: {exception.Message} @ {exception.StackTrace}";
            this.navigation.DisplayAlertAsync("Error", errorMessage);
        }
    }
}
