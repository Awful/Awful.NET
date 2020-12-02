// <copyright file="AwfulClientException.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Core.Entities.Web;

namespace Awful.Core.Exceptions
{
    /// <summary>
    /// Awful Client Exception.
    /// Handled inside raw AwfulClient requests.
    /// </summary>
    public class AwfulClientException : Exception
    {
        /// <summary>
        /// Used to get the AwfulClient.
        /// </summary>
        public const string AwfulClientKey = "AwfulClient";

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClientException"/> class.
        /// </summary>
        /// <param name="message">Inner Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        /// <param name="result"><see cref="Result"/> object.</param>
        public AwfulClientException(string message, Exception innerException, Result result)
            : base(message, innerException)
        {
            this.Data.Add(AwfulClientKey, result);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClientException"/> class.
        /// </summary>
        /// <param name="message">Inner Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        /// <param name="result"><see cref="Result"/> object.</param>
        public AwfulClientException(Exception innerException, Result result)
            : base(Awful.Core.Resources.ExceptionMessages.AwfulClientError, innerException)
        {
            this.Data.Add(AwfulClientKey, result);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClientException"/> class.
        /// </summary>
        public AwfulClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClientException"/> class.
        /// </summary>
        /// <param name="result"><see cref="Result"/> object.</param>
        public AwfulClientException(Result result)
        {
            this.Data.Add(AwfulClientKey, result);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClientException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        public AwfulClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulClientException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        /// <param name="innerException">Internal Exception.</param>
        public AwfulClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
