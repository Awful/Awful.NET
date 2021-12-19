// <copyright file="User.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Posts
{
    /// <summary>
    /// SA Forum Post User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the avatar link.
        /// </summary>
        public string? AvatarLink { get; set; }

        /// <summary>
        /// Gets a value indicating whether the avatar is visible or not.
        /// </summary>
        public bool IsAvatarHidden
        {
            get { return string.IsNullOrEmpty(this.AvatarLink); }
        }

        /// <summary>
        /// Gets or sets the avatar title.
        /// </summary>
        public string? AvatarTitle { get; set; }

        /// <summary>
        /// Gets or sets the avatar html.
        /// </summary>
        public string? AvatarHtml { get; set; }

        /// <summary>
        /// Gets or sets the Avatar Gang tag link.
        /// </summary>
        public string? AvatarGangTagLink { get; set; }

        /// <summary>
        /// Gets or sets the date joined.
        /// </summary>
        public DateTime DateJoined { get; set; }

        /// <summary>
        /// Gets or sets the users roles.
        /// </summary>
        public string? Roles { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user has roles.
        /// </summary>
        public bool HasRoles => !string.IsNullOrEmpty(this.Roles);

        /// <summary>
        /// Gets or sets the users title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public long Id { get; set; }
    }
}
