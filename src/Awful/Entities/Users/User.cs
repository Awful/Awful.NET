// <copyright file="User.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Entities.Users
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
        /// Gets or sets the user pic link.
        /// </summary>
        public string? UserPicLink { get; set; }

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
        /// Gets or sets the profile link.
        /// </summary>
        public string? ProfileLink { get; set; }

        /// <summary>
        /// Gets or sets the private message link.
        /// </summary>
        public string? PrivateMessageLink { get; set; }

        /// <summary>
        /// Gets or sets the post history link.
        /// </summary>
        public string? PostHistoryLink { get; set; }

        /// <summary>
        /// Gets or sets the rap sheet link.
        /// </summary>
        public string? RapSheetLink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if you can send a
        /// private message to this user.
        /// </summary>
        public bool CanSendPrivateMessage { get; set; }

        /// <summary>
        /// Gets or sets the Icq Contact string.
        /// </summary>
        public string? IcqContactString { get; set; }

        /// <summary>
        /// Gets or sets the Aim Contact String.
        /// </summary>
        public string? AimContactString { get; set; }

        /// <summary>
        /// Gets or sets the Yahoo Contact string.
        /// </summary>
        public string? YahooContactString { get; set; }

        /// <summary>
        /// Gets or sets the home page string.
        /// </summary>
        public string? HomePageString { get; set; }

        /// <summary>
        /// Gets or sets the users post count.
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// Gets or sets the last post date of the user.
        /// </summary>
        public string? LastPostDate { get; set; }

        /// <summary>
        /// Gets or sets the location of the user.
        /// Defaults to Unknown.
        /// </summary>
        public string? Location { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets info about the user.
        /// </summary>
        public string? AboutUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a mod.
        /// </summary>
        public bool IsMod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a admin.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the users roles.
        /// </summary>
        public string? Roles { get; set; }

        /// <summary>
        /// Gets or sets the users title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a current user post.
        /// </summary>
        public bool IsCurrentUserPost { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the postrate of the user.
        /// </summary>
        public string? PostRate { get; set; }

        /// <summary>
        /// Gets or sets the users seller rating.
        /// </summary>
        public string? SellerRating { get; set; }
    }
}
