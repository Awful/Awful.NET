// <copyright file="SortedIndexPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;

namespace Awful.Core.Entities.JSON
{
    /// <summary>
    /// Index page stats.
    /// </summary>
    public class SortedIndexPage
    {
        /// <summary>
        /// Gets or sets the stats of a given forum.
        /// </summary>
        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        /// <summary>
        /// Gets or sets the user asking for the forum info.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the list of forums, listed by category.
        /// </summary>
        [JsonProperty("forums")]
        public List<Forum> ForumCategories { get; set; }

        /// <summary>
        /// Gets or sets the list of forums, flattened.
        /// </summary>
        [JsonProperty("forums")]
        public List<Forum> Forums { get; set;  }
    }
}
