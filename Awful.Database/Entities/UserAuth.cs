// <copyright file="UserAuth.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace Awful.Database.Entities
{
    /// <summary>
    /// User Auth Entities. Used for handling SA Auth.
    /// </summary>
    public class UserAuth
    {
        /// <summary>
        /// Gets or sets the User Auth Id.
        /// </summary>
        [Key]
        public int UserAuthId { get; set; }

        /// <summary>
        /// Gets or sets the Users UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the User Avatar.
        /// </summary>
        public string AvatarLink { get; set; }

        /// <summary>
        /// Gets or sets the User Auth Cookies path.
        /// </summary>
        public string CookiePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the user can recieve PMs.
        /// </summary>
        public bool RecievePM { get; set; }

        /// <summary>
        /// Gets or sets the Users Auth Cookies.
        /// </summary>
        public CookieContainer AuthCookies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is the default.
        /// </summary>
        public bool IsDefaultUser { get; set; }
    }
}
