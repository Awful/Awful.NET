// <copyright file="BanPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.Bans
{
    /// <summary>
    /// Something Awful Ban Page.
    /// </summary>
    public class BanPage : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BanPage"/> class.
        /// </summary>
        /// <param name="currentPage">The Current Ban page.</param>
        /// <param name="totalPages">The Total pages.</param>
        /// <param name="items">The ban items on the page.</param>
        public BanPage(int currentPage, int totalPages, List<BanItem> items)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.Bans = items;
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets the banned users.
        /// </summary>
        public List<BanItem> Bans { get; }
    }
}
