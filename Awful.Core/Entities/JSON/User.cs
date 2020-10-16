// <copyright file="User.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;

namespace Awful.Core.Entities.JSON
{
    /// <summary>
    /// Something Awful User Object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [JsonProperty("userid")]
        public long Userid { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the homepage.
        /// </summary>
        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        /// <summary>
        /// Gets or sets the ICQ name.
        /// </summary>
        [JsonProperty("icq")]
        public string Icq { get; set; }

        /// <summary>
        /// Gets or sets the AIM id.
        /// </summary>
        [JsonProperty("aim")]
        public string Aim { get; set; }

        /// <summary>
        /// Gets or sets the Yahoo id.
        /// </summary>
        [JsonProperty("yahoo")]
        public string Yahoo { get; set; }

        /// <summary>
        /// Gets or sets the users gender.
        /// </summary>
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the users title.
        /// </summary>
        [JsonProperty("usertitle")]
        public string Usertitle { get; set; }

        /// <summary>
        /// Gets or sets the join date (long).
        /// </summary>
        [JsonProperty("joindate")]
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
        [JsonProperty("lastpost")]
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
        [JsonProperty("posts")]
        public long Posts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can recieve PMs.
        /// </summary>
        [JsonProperty("receivepm")]
        public bool Receivepm { get; set; }

        /// <summary>
        /// Gets or sets the number of posts the user makes per day.
        /// </summary>
        [JsonProperty("postsperday")]
        public double Postsperday { get; set; }

        /// <summary>
        /// Gets or sets the role of a user.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the biography of a user.
        /// </summary>
        [JsonProperty("biography")]
        public string Biography { get; set; } = "None";

        /// <summary>
        /// Gets or sets the location of a user.
        /// </summary>
        [JsonProperty("location")]
        public string Location { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets the interests of a user.
        /// </summary>
        [JsonProperty("interests")]
        public string Interests { get; set; }

        /// <summary>
        /// Gets or sets the occupation of a user.
        /// </summary>
        [JsonProperty("occupation")]
        public string Occupation { get; set; }

        /// <summary>
        /// Gets or sets the users picture.
        /// </summary>
        [JsonProperty("picture")]
        public string Picture { get; set; }
    }
}
