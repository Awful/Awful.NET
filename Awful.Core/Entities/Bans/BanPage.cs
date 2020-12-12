// <copyright file="BanPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Awful.Core.Entities.Web;

namespace Awful.Core.Entities.Bans
{
    /// <summary>
    /// Something Awful Ban Page.
    /// </summary>
    public class BanPage : SAItem
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; } = 1;

        /// <summary>
        /// Gets or sets the banned users.
        /// </summary>
        public List<BanItem> Bans { get; } = new List<BanItem>();
    }
}
