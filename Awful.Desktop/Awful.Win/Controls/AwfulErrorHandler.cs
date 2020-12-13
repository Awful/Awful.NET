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

namespace Awful.Win.Controls
{
    /// <summary>
    /// Awful Error Handler.
    /// </summary>
    public class AwfulErrorHandler : IAwfulErrorHandler
    {
        private IAwfulNavigation navigation;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulErrorHandler"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation.</param>
        public AwfulErrorHandler(IAwfulNavigation navigation)
        {
            this.navigation = navigation;
        }

        public void HandleError(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            string errorMessage;

            if (exception is AwfulClientException awfulClientException)
            {
                if (awfulClientException.InnerException is PaywallException paywallException)
                {
                    errorMessage = paywallException.Message;
                }
                else
                {
                    var result = (Result)awfulClientException.Data[AwfulClientException.AwfulClientKey];
                    errorMessage = !string.IsNullOrEmpty(result.ErrorText) ? result.ErrorText : $"AwfulClient failed to make a request: {result.Message.StatusCode} - {result.Message.ReasonPhrase} - {result.AbsoluteEndpoint}";
                }
            }
            else
            {
                errorMessage = $"An {exception.GetType().FullName} was thrown: {exception.Message} @ {exception.StackTrace}";
            }

            this.navigation.DisplayAlertAsync("Error", errorMessage);
        }
    }
}
