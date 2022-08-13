// <copyright file="PaywallException.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Exceptions
{
    /// <summary>
    /// Exception thrown when a user hits a paywall page.
    /// </summary>
    public class PaywallException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaywallException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        public PaywallException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaywallException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        /// <param name="innerException">Internal Exception.</param>
        public PaywallException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaywallException"/> class.
        /// Exception thrown when a user hits a paywall page.
        /// </summary>
        public PaywallException()
        {
        }
    }
}