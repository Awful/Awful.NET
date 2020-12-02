// <copyright file="AwfulParserException.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Core.Entities;
using Awful.Core.Entities.Web;

namespace Awful.Core.Exceptions
{
    /// <summary>
    /// Awful Parser Exception.
    /// Thrown when there's an error parsing an item from SA.
    /// </summary>
    public class AwfulParserException : Exception
    {
        /// <summary>
        /// Used to get the AwfulClient.
        /// </summary>
        public const string AwfulParserKey = "AwfulParser";

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulParserException"/> class.
        /// </summary>
        /// <param name="message">Inner Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        /// <param name="item"><see cref="SAItem"/> object.</param>
        public AwfulParserException(string message, Exception innerException, SAItem item)
            : base(message, innerException)
        {
            this.Data.Add(AwfulParserKey, item);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulParserException"/> class.
        /// </summary>
        /// <param name="message">Inner Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        /// <param name="item"><see cref="SAItem"/> object.</param>
        public AwfulParserException(Exception innerException, SAItem item)
            : base(Awful.Core.Resources.ExceptionMessages.AwfulClientError, innerException)
        {
            this.Data.Add(AwfulParserKey, item);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulParserException"/> class.
        /// </summary>
        public AwfulParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulParserException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        public AwfulParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulParserException"/> class.
        /// </summary>
        /// <param name="message">Inner message.</param>
        /// <param name="innerException">Internal Exception.</param>
        public AwfulParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
