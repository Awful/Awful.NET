// <copyright file="Thread.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Something Awful Thread.
    /// </summary>
    public class Thread
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Thread"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="name">Name.</param>
        /// <param name="author">Author.</param>
        /// <param name="authorId">authorId.</param>
        /// <param name="starColor">starColor.</param>
        /// <param name="canMarkAsUnread">canMarkAsUnread.</param>
        /// <param name="isLocked">isLocked.</param>
        /// <param name="isBookmark">isBookmark.</param>
        /// <param name="isSticky">isSticky.</param>
        /// <param name="isAnnouncement">isAnnouncement.</param>
        /// <param name="isArchive">isArchive.</param>
        /// <param name="hasSeen">hasSeen.</param>
        /// <param name="killedOn">killedOn.</param>
        /// <param name="killedBy">killedBy.</param>
        /// <param name="killedById">killedById.</param>
        /// <param name="replyCount">replyCount.</param>
        /// <param name="repliesSinceLastOpen">repliesSinceLastOpen.</param>
        /// <param name="viewCount">viewCount.</param>
        /// <param name="rating">rating.</param>
        /// <param name="totalRatingVotes">totalRatingVotes.</param>
        /// <param name="ratingImageEndpoint">ratingImageEndpoint.</param>
        /// <param name="imageIconEndpoint">imageIconEndpoint.</param>
        /// <param name="storeImageIconEndpoint">storeImageIconEndpoint.</param>
        public Thread(
            int id,
            string name,
            string author,
            long authorId,
            string? starColor = null,
            bool canMarkAsUnread = false,
            bool isLocked = false,
            bool isBookmark = false,
            bool isSticky = false,
            bool isAnnouncement = false,
            bool isArchive = false,
            bool hasSeen = false,
            DateTime? killedOn = null,
            string? killedBy = null,
            long killedById = 0,
            int replyCount = 0,
            int repliesSinceLastOpen = 0,
            int viewCount = 0,
            decimal rating = 0,
            int totalRatingVotes = 0,
            string? ratingImageEndpoint = null,
            string? imageIconEndpoint = null,
            string? storeImageIconEndpoint = null)
        {
            this.IsLocked = isLocked;
            this.CanMarkAsUnread = canMarkAsUnread;
            this.RepliesSinceLastOpened = repliesSinceLastOpen;
            this.IsBookmark = isBookmark;
            this.HasSeen = hasSeen;
            this.StarColor = starColor ?? string.Empty;
            this.IsSticky = isSticky;
            this.IsAnnouncement = isAnnouncement;
            this.IsArchived = isArchive;
            this.ThreadId = id;
            this.Author = author;
            this.AuthorId = authorId;
            this.KilledBy = killedBy ?? string.Empty;
            this.KilledById = killedById;
            this.Name = name;
            this.TotalRatingVotes = totalRatingVotes;
            this.ReplyCount = replyCount;
            this.ViewCount = viewCount;
            this.Rating = rating;
            this.KilledOn = killedOn ?? DateTime.MinValue;
            this.RatingImageEndpoint = ratingImageEndpoint ?? string.Empty;
            this.RatingImage = Path.GetFileNameWithoutExtension(this.RatingImageEndpoint) ?? string.Empty;
            this.ImageIconEndpoint = imageIconEndpoint ?? string.Empty;
            this.ImageIconLocation = Path.GetFileNameWithoutExtension(this.ImageIconEndpoint) ?? string.Empty;
            this.StoreImageIconEndpoint = storeImageIconEndpoint ?? string.Empty;
            this.StoreImageIconLocation = Path.GetFileNameWithoutExtension(this.StoreImageIconEndpoint) ?? string.Empty;
        }

        /// <summary>
        /// Gets the thread id.
        /// </summary>
        public int ThreadId { get; }

        /// <summary>
        /// Gets the name of the thread.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the image icon endpoint.
        /// </summary>
        public string ImageIconEndpoint { get; }

        /// <summary>
        /// Gets the image icon location.
        /// </summary>
        public string ImageIconLocation { get; }

        /// <summary>
        /// Gets the store image icon endpoint.
        /// </summary>
        public string StoreImageIconEndpoint { get; }

        /// <summary>
        /// Gets the store image icon location.
        /// </summary>
        public string StoreImageIconLocation { get; }

        /// <summary>
        /// Gets the author of the thread.
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// Gets the author id.
        /// </summary>
        public long AuthorId { get; }

        /// <summary>
        /// Gets the reply count.
        /// </summary>
        public int ReplyCount { get; }

        /// <summary>
        /// Gets the view count.
        /// </summary>
        public int ViewCount { get; }

        /// <summary>
        /// Gets the rating of the thread.
        /// </summary>
        public decimal Rating { get; }

        /// <summary>
        /// Gets the total rating votes.
        /// </summary>
        public int TotalRatingVotes { get; }

        /// <summary>
        /// Gets the rating image.
        /// </summary>
        public string RatingImage { get; }

        /// <summary>
        /// Gets the rating image endpoint.
        /// </summary>
        public string RatingImageEndpoint { get; }

        /// <summary>
        /// Gets the user name who killed the thread.
        /// </summary>
        public string KilledBy { get; }

        /// <summary>
        /// Gets the user id who killed the thread.
        /// </summary>
        public long KilledById { get; }

        /// <summary>
        /// Gets the date the thread was killed.
        /// </summary>
        public DateTime KilledOn { get; }

        /// <summary>
        /// Gets the date the thread was killed.
        /// </summary>
        public string KilledOnHours => this.KilledOn.ToString("HH:mm");

        /// <summary>
        /// Gets the date the thread was killed.
        /// </summary>
        public string KilledOnDay => this.KilledOn.ToString("dd-MM-yy");

        /// <summary>
        /// Gets a value indicating whether the thread is archived.
        /// </summary>
        public bool IsArchived { get; }

        /// <summary>
        /// Gets a value indicating whether the thread is a sticky.
        /// </summary>
        public bool IsSticky { get; }

        /// <summary>
        /// Gets a value indicating whether the thread is locked.
        /// </summary>
        public bool IsLocked { get; }

        /// <summary>
        /// Gets a value indicating whether the thread is an announcement.
        /// </summary>
        public bool IsAnnouncement { get; }

        /// <summary>
        /// Gets a value indicating whether the thread can be marked as unread.
        /// </summary>
        public bool CanMarkAsUnread { get; }

        /// <summary>
        /// Gets the number of replies since last opened.
        /// </summary>
        public int RepliesSinceLastOpened { get; }

        /// <summary>
        /// Gets a value indicating whether the thread has been seen.
        /// </summary>
        public bool HasSeen { get; }

        /// <summary>
        /// Gets a value indicating whether the thread is a bookmarked thread.
        /// </summary>
        public bool IsBookmark { get; }

        /// <summary>
        /// Gets the star color.
        /// </summary>
        public string StarColor { get; }
    }
}
