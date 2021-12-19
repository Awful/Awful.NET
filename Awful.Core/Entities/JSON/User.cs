// <copyright file="User.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Awful.Core.Entities.JSON
{
    /// <summary>
    /// Something Awful User Object.
    /// </summary>
    public class User : SAItem
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [JsonPropertyName("userid")]
        public long Userid { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        [JsonPropertyName("homepage")]
        public string? Homepage { get; set; }

        /// <summary>
        /// Gets or sets the ICQ name.
        /// </summary>
        [JsonPropertyName("icq")]
        public string? Icq { get; set; }

        /// <summary>
        /// Gets or sets the AIM id.
        /// </summary>
        [JsonPropertyName("aim")]
        public string? Aim { get; set; }

        /// <summary>
        /// Gets or sets the Yahoo id.
        /// </summary>
        [JsonPropertyName("yahoo")]
        public string? Yahoo { get; set; }

        /// <summary>
        /// Gets or sets the users gender.
        /// </summary>
        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the users title.
        /// </summary>
        [JsonPropertyName("usertitle")]
        public string? Usertitle { get; set; }

        /// <summary>
        /// Gets the Avatar.
        /// </summary>
        public string Avatar
        {
            get
            {
                if (this.Usertitle == null)
                {
                    return string.Empty;
                }

                var captures = Regex.Match(this.Usertitle, @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?");
                var avatarLink = captures.Captures.Count > 0 ? captures.Captures[0].Value : string.Empty;
                return avatarLink;
            }
        }

        /// <summary>
        /// Gets or sets the join date (long).
        /// </summary>
        [JsonPropertyName("joindate")]
        public long Joindate { get; set; }

        /// <summary>
        /// Gets the join date from a user.
        /// </summary>
        public DateTime JoinDate
        {
            get
            {
                if (this.Joindate <= 0)
                {
                    return default;
                }

                return DateTimeOffset.FromUnixTimeSeconds(this.Joindate).DateTime;
            }
        }

        /// <summary>
        /// Gets or sets the last post from the user (long).
        /// </summary>
        [JsonPropertyName("lastpost")]
        public long Lastpost { get; set; }

        /// <summary>
        /// Gets the last post date from a user.
        /// </summary>
        public DateTime LastPostDate
        {
            get
            {
                if (this.Lastpost <= 0)
                {
                    return default;
                }

                return DateTimeOffset.FromUnixTimeSeconds(this.Lastpost).DateTime;
            }
        }

        /// <summary>
        /// Gets or sets the number of posts a user has made.
        /// </summary>
        [JsonPropertyName("posts")]
        public long Posts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can recieve PMs.
        /// </summary>
        [JsonPropertyName("receivepm")]
        public bool Receivepm { get; set; }

        /// <summary>
        /// Gets or sets the number of posts the user makes per day.
        /// </summary>
        [JsonPropertyName("postsperday")]
        public double Postsperday { get; set; }

        /// <summary>
        /// Gets or sets the role of a user.
        /// </summary>
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the biography of a user.
        /// </summary>
        [JsonPropertyName("biography")]
        public string Biography { get; set; } = "None";

        /// <summary>
        /// Gets or sets the location of a user.
        /// </summary>
        [JsonPropertyName("location")]
        public string Location { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets the interests of a user.
        /// </summary>
        [JsonPropertyName("interests")]
        public string? Interests { get; set; }

        /// <summary>
        /// Gets or sets the occupation of a user.
        /// </summary>
        [JsonPropertyName("occupation")]
        public string? Occupation { get; set; }

        /// <summary>
        /// Gets or sets the users picture.
        /// </summary>
        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
    }
}
