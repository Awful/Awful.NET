// <copyright file="Stats.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

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
        [JsonPropertyName("archived_posts")]
        public long ArchivedPosts { get; set; }

        /// <summary>
        /// Gets or sets the number of archived threads.
        /// </summary>
        [JsonPropertyName("archived_threads")]
        public long ArchivedThreads { get; set; }

        /// <summary>
        /// Gets or sets the number of banned users.
        /// </summary>
        [JsonPropertyName("banned_users")]
        public long BannedUsers { get; set; }

        /// <summary>
        /// Gets or sets the total number of banned users.
        /// </summary>
        [JsonPropertyName("banned_users_total")]
        public long BannedUsersTotal { get; set; }

        /// <summary>
        /// Gets or sets the number of total online registered users.
        /// </summary>
        [JsonPropertyName("online_registered")]
        public long OnlineRegistered { get; set; }

        /// <summary>
        /// Gets or sets the total number of online users.
        /// </summary>
        [JsonPropertyName("online_total")]
        public long OnlineTotal { get; set; }

        /// <summary>
        /// Gets or sets the numbers of unique posts.
        /// </summary>
        [JsonPropertyName("unique_posts")]
        public long UniquePosts { get; set; }

        /// <summary>
        /// Gets or sets the number of unique threads.
        /// </summary>
        [JsonPropertyName("unique_threads")]
        public long UniqueThreads { get; set; }

        /// <summary>
        /// Gets or sets the numbers of users.
        /// </summary>
        [JsonPropertyName("usercount")]
        public long Usercount { get; set; }
    }
}
