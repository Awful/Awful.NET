// <copyright file="AuthResult.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Awful.Core.Entities.Users;

namespace Awful.Core.Entities.Web
{
    /// <summary>
    /// The AuthRequest request.
    /// </summary>
    public class AuthResult : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthResult"/> class.
        /// </summary>
        /// <param name="container">The CookieContainer.</param>
        /// <param name="isSuccess">If the request was successful.</param>
        /// <param name="error">The error.</param>
        public AuthResult(CookieContainer container, bool isSuccess = false, string error = "")
        {
            this.IsSuccess = isSuccess;
            this.AuthenticationCookieContainer = container;
            this.Error = error;
        }

        /// <summary>
        /// Gets or sets a value indicating whether if the request we've recieved was gotten successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets if errored on authentication, will contain the error message.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the current user for this login.
        /// </summary>
        public Awful.Core.Entities.JSON.User CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the serialized authentication cookie from logging in.
        /// </summary>
        public CookieContainer AuthenticationCookieContainer { get; set; }
    }
}
