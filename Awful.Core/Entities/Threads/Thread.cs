// <copyright file="Thread.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Something Awful Thread.
    /// </summary>
    public class Thread
    {
        /// <summary>
        /// Gets or sets the name of the thread.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the thread.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the image icon endpoint.
        /// </summary>
        public string ImageIconEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image icon location.
        /// </summary>
        public string ImageIconLocation { get; set; }

        /// <summary>
        /// Gets or sets the store image icon endpoint.
        /// </summary>
        public string StoreImageIconEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the store image icon location.
        /// </summary>
        public string StoreImageIconLocation { get; set; }

        /// <summary>
        /// Gets or sets the author of the thread.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        public long AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the reply count.
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// Gets or sets the view count.
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Gets or sets the rating of the thread.
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// Gets or sets the total rating votes.
        /// </summary>
        public int TotalRatingVotes { get; set; }

        /// <summary>
        /// Gets or sets the rating image.
        /// </summary>
        public string RatingImage { get; set; }

        /// <summary>
        /// Gets or sets the rating image endpoint.
        /// </summary>
        public string RatingImageEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the user name who killed the thread.
        /// </summary>
        public string KilledBy { get; set; }

        /// <summary>
        /// Gets or sets the user id who killed the thread.
        /// </summary>
        public long KilledById { get; set; }

        /// <summary>
        /// Gets or sets the date the thread was killed.
        /// </summary>
        public DateTime KilledOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is a sticky.
        /// </summary>
        public bool IsSticky { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is an announcement.
        /// </summary>
        public bool IsAnnouncement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread has been viewed.
        /// </summary>
        public bool HasBeenViewed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread can be marked as unread.
        /// </summary>
        public bool CanMarkAsUnread { get; set; }

        /// <summary>
        /// Gets or sets the number of replies since last opened.
        /// </summary>
        public int RepliesSinceLastOpened { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the scroll to post.
        /// </summary>
        public int ScrollToPost { get; set; }

        /// <summary>
        /// Gets or sets the scroll to post string.
        /// </summary>
        public string ScrollToPostString { get; set; }

        /// <summary>
        /// Gets or sets the thread id.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread has been seen.
        /// </summary>
        public bool HasSeen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is a bookmarked thread.
        /// </summary>
        public bool IsBookmark { get; set; }

        /// <summary>
        /// Gets or sets the star color.
        /// </summary>
        public string StarColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a private message.
        /// </summary>
        public bool IsPrivateMessage { get; set; }
    }
}
