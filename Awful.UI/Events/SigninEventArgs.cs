// <copyright file="SigninEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Database.Entities;

namespace Awful.UI.Events
{
    /// <summary>
    /// Signin Event Arguments.
    /// </summary>
    public class SigninEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn { get; set; }

        /// <summary>
        /// Gets or sets the users auth.
        /// </summary>
        public UserAuth User { get; set; }
    }
}
