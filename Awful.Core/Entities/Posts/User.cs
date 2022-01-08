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
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="username">Username.</param>
        /// <param name="dateJoined">Date Joined.</param>
        /// <param name="title">Title.</param>
        /// <param name="roles">Roles.</param>
        /// <param name="avatarTitle">Avatar Title.</param>
        /// <param name="avatarHtml">Avatar Html.</param>
        /// <param name="avatarLink">Link.</param>
        /// <param name="avatarGangTagLink">Gang Tag Link.</param>
        public User(
            long id,
            string username,
            DateTime? dateJoined,
            string? title = "",
            string? roles = "",
            string? avatarTitle = "",
            string? avatarHtml = "",
            string? avatarLink = "",
            string? avatarGangTagLink = "")
        {
            this.Id = id;
            this.Username = username;
            this.DateJoined = dateJoined;
            this.Title = title;
            this.Roles = roles;
            this.AvatarTitle = avatarTitle;
            this.AvatarHtml = avatarHtml;
            this.AvatarLink = avatarLink;
            this.AvatarGangTagLink = avatarGangTagLink;
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the avatar link.
        /// </summary>
        public string? AvatarLink { get; }

        /// <summary>
        /// Gets a value indicating whether the avatar is visible or not.
        /// </summary>
        public bool IsAvatarHidden
        {
            get { return string.IsNullOrEmpty(this.AvatarLink); }
        }

        /// <summary>
        /// Gets the avatar title.
        /// </summary>
        public string? AvatarTitle { get; }

        /// <summary>
        /// Gets the avatar html.
        /// </summary>
        public string? AvatarHtml { get; }

        /// <summary>
        /// Gets the Avatar Gang tag link.
        /// </summary>
        public string? AvatarGangTagLink { get;  }

        /// <summary>
        /// Gets the date joined.
        /// </summary>
        public DateTime? DateJoined { get; }

        /// <summary>
        /// Gets the users roles.
        /// </summary>
        public string? Roles { get; }

        /// <summary>
        /// Gets a value indicating whether the user has roles.
        /// </summary>
        public bool HasRoles => !string.IsNullOrEmpty(this.Roles);

        /// <summary>
        /// Gets the users title.
        /// </summary>
        public string? Title { get;  }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public long Id { get;  }
    }
}
