// <copyright file="AuthResult.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Net;

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
        /// <param name="user">User.</param>
        /// <param name="isSuccess">If the request was successful.</param>
        /// <param name="error">The error.</param>
        public AuthResult(CookieContainer container, JSON.User? user = null, string error = "")
        {
            this.IsSuccess = string.IsNullOrEmpty(error);
            this.AuthenticationCookieContainer = container;
            this.Error = error;
            this.CurrentUser = user;
        }

        /// <summary>
        /// Gets a value indicating whether if the request we've recieved was gotten successfully.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets if errored on authentication, will contain the error message.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets a value indicating whether the auth result errored.
        /// </summary>
        public bool IsError => !string.IsNullOrEmpty(this.Error);

        /// <summary>
        /// Gets the current user for this login.
        /// </summary>
        public Awful.Core.Entities.JSON.User? CurrentUser { get; }

        /// <summary>
        /// Gets the serialized authentication cookie from logging in.
        /// </summary>
        public CookieContainer AuthenticationCookieContainer { get; }
    }
}
