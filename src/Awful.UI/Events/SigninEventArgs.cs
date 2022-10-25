// <copyright file="SigninEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.UI.Entities;

namespace Awful.UI.Events
{
    /// <summary>
    /// Signin Event Arguments.
    /// </summary>
    public class SigninEventArgs : EventArgs
    {
        public SigninEventArgs(bool isSignedIn, UserAuth? user)
        {
            IsSignedIn = isSignedIn;
            User = user;
        }

        /// <summary>
        /// Gets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn { get; }

        /// <summary>
        /// Gets the users auth.
        /// </summary>
        public UserAuth? User { get; }
    }
}
