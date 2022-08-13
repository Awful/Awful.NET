// <copyright file="BanItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.Bans
{
    /// <summary>
    /// Banned User Item.
    /// </summary>
    public class BanItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BanItem"/> class.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <param name="type">Type of ban.</param>
        /// <param name="jerkId">Jerk Id.</param>
        /// <param name="horribleJerk">Horrible Jerk.</param>
        /// <param name="date">Date of the ban.</param>
        /// <param name="punishmentReason">Punishment Reason, may be HTML.</param>
        /// <param name="approvedById">Approved by Id.</param>
        /// <param name="approvedBy">Approved By.</param>
        /// <param name="requestedById">Requested by id.</param>
        /// <param name="requestedBy">Requested by.</param>
        public BanItem(int postId, string type, int jerkId, string horribleJerk, DateTime date, string punishmentReason, int approvedById, string approvedBy, int requestedById = 0, string? requestedBy = null)
        {
            this.PostId = postId;
            this.Type = type;
            this.Date = date;
            this.HorribleJerk = horribleJerk;
            this.HorribleJerkId = jerkId;
            this.Date = date;
            this.PunishmentReason = punishmentReason;
            this.ApprovedBy = approvedBy;
            this.ApprovedById = approvedById;
            this.RequestedBy = requestedBy;
            this.RequestedById = requestedById;
        }

        /// <summary>
        /// Gets the type of ban.
        /// </summary>
        public string? Type { get; }

        /// <summary>
        /// Gets the id of the post.
        /// </summary>
        public int PostId { get; }

        /// <summary>
        /// Gets the date of the ban.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets the user who is banned.
        /// </summary>
        public string? HorribleJerk { get; }

        /// <summary>
        /// Gets the id of the user who is banned.
        /// </summary>
        public int HorribleJerkId { get; }

        /// <summary>
        /// Gets the reason for the ban.
        /// </summary>
        public string PunishmentReason { get; }

        /// <summary>
        /// Gets the user who requested the ban.
        /// </summary>
        public string? RequestedBy { get; }

        /// <summary>
        /// Gets the id of the user who requested the ban.
        /// </summary>
        public int RequestedById { get; }

        /// <summary>
        /// Gets the user who approved the ban.
        /// </summary>
        public string ApprovedBy { get; }

        /// <summary>
        /// Gets the id of the user who approved the ban.
        /// </summary>
        public int ApprovedById { get; }

        /// <summary>
        /// Gets a value indicating whether the user is permabanned.
        /// </summary>
        public bool IsPermaBanned
        {
            get { return this.Type?.ToUpperInvariant() == "PERMABAN"; }
        }
    }
}
