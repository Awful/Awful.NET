// <copyright file="UserAuthenticationException.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Exceptions
{
    /// <summary>
    /// Exception thrown when a user an auth error.
    /// </summary>
    public class UserAuthenticationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        public UserAuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        /// <param name="innerException">Internal Exception.</param>
        public UserAuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticationException"/> class.
        /// Exception thrown when a user hits an auth error.
        /// </summary>
        public UserAuthenticationException()
        {
        }
    }
}