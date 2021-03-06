﻿// <copyright file="Stats.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;

namespace Awful.Core.Entities.JSON
{
    /// <summary>
    /// The user stats for a given forum.
    /// </summary>
    public class Stats
    {
        /// <summary>
        /// Gets or sets the number of archived posts.
        /// </summary>
        [JsonProperty("archived_posts")]
        public long ArchivedPosts { get; set; }

        /// <summary>
        /// Gets or sets the number of archived threads.
        /// </summary>
        [JsonProperty("archived_threads")]
        public long ArchivedThreads { get; set; }

        /// <summary>
        /// Gets or sets the number of banned users.
        /// </summary>
        [JsonProperty("banned_users")]
        public long BannedUsers { get; set; }

        /// <summary>
        /// Gets or sets the total number of banned users.
        /// </summary>
        [JsonProperty("banned_users_total")]
        public long BannedUsersTotal { get; set; }

        /// <summary>
        /// Gets or sets the number of total online registered users.
        /// </summary>
        [JsonProperty("online_registered")]
        public long OnlineRegistered { get; set; }

        /// <summary>
        /// Gets or sets the total number of online users.
        /// </summary>
        [JsonProperty("online_total")]
        public long OnlineTotal { get; set; }

        /// <summary>
        /// Gets or sets the numbers of unique posts.
        /// </summary>
        [JsonProperty("unique_posts")]
        public long UniquePosts { get; set; }

        /// <summary>
        /// Gets or sets the number of unique threads.
        /// </summary>
        [JsonProperty("unique_threads")]
        public long UniqueThreads { get; set; }

        /// <summary>
        /// Gets or sets the numbers of users.
        /// </summary>
        [JsonProperty("usercount")]
        public long Usercount { get; set; }
    }
}
