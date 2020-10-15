// <copyright file="Moderator.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;

namespace Awful.Core.Entities.JSON
{
    /// <summary>
    /// The Moderator of a forum.
    /// </summary>
    public class Moderator
    {
        /// <summary>
        /// Gets or sets the user id of a moderator.
        /// </summary>
        [JsonProperty("userid")]
        public long Userid { get; set; }

        /// <summary>
        /// Gets or sets the username of a moderator.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
