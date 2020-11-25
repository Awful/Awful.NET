// <copyright file="Forum.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;

namespace Awful.Core.Entities.JSON
{
    /// <summary>
    /// Something Awful Forum Object.
    /// </summary>
    public class Forum
    {
        /// <summary>
        /// Gets or sets the id of a forum.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the parent id of a forum.
        /// </summary>
        public int? ParentForumId { get; set; }

        /// <summary>
        /// Gets or sets the parent id category.
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short version of the title.
        /// </summary>
        [JsonProperty("title_short")]
        public string TitleShort { get; set; }

        /// <summary>
        /// Gets or sets the description of a forum.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the forum has threads.
        /// </summary>
        [JsonProperty("has_threads")]
        public bool HasThreads { get; set; }

        /// <summary>
        /// Gets or sets the icon for a forum.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the list of subforums.
        /// </summary>
        [JsonProperty("sub_forums")]
        public virtual List<Forum> SubForums { get; set; } = new List<Forum>();

        /// <summary>
        /// Gets or sets a value indicating whether the forum is a favorite.
        /// </summary>
        public bool IsFavorited { get; set; }

        /// <summary>
        /// Gets or sets the Parent Forum.
        /// </summary>
        public virtual Forum ParentForum { get; set; }

        /// <summary>
        /// Gets or sets the list of moderators for a given forum.
        /// </summary>
        [JsonProperty("moderators")]
        public List<Moderator> Moderators { get; set; } = new List<Moderator>();
    }
}
