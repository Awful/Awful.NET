// <copyright file="SettingOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Awful.Webview.Entities.Themes;

namespace Awful.Database.Entities
{
    /// <summary>
    /// Manages the Settings for the apps.
    /// </summary>
    public class SettingOptions
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Last Bookmark Refresh Time.
        /// </summary>
        public DateTime LastBookmarkRefreshTime { get; set; }

        /// <summary>
        /// Gets or sets the default theme options.
        /// </summary>
        public DefaultOptions DefaultThemeOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable background tasks.
        /// </summary>
        public bool EnableBackgroundTasks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable bokmark refresh on page.
        /// </summary>
        public bool EnableBookmarkRefreshOnPage { get; set; }

        /// <summary>
        /// Gets or sets how long to refresh the bookmarks on the page.
        /// </summary>
        public int BookmarkOnPageRefreshTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or sets how long to refresh the bookmarks in the background.
        /// </summary>
        public int BookmarkInBackgroundRefreshTimeInMinutes { get; set; }
    }
}
