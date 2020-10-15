// <copyright file="BanItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Bans
{
    /// <summary>
    /// Banned User Item.
    /// </summary>
    public class BanItem
    {
        /// <summary>
        /// Gets or sets the type of ban.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the id of the post.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the date of the ban.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the user who is banned.
        /// </summary>
        public string HorribleJerk { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who is banned.
        /// </summary>
        public int HorribleJerkId { get; set; }

        /// <summary>
        /// Gets or sets the reason for the ban.
        /// </summary>
        public string PunishmentReason { get; set; }

        /// <summary>
        /// Gets or sets the user who requested the ban.
        /// </summary>
        public string RequestedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who requested the ban.
        /// </summary>
        public int RequestedById { get; set; }

        /// <summary>
        /// Gets or sets the user who approved the ban.
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who approved the ban.
        /// </summary>
        public int ApprovedById { get; set; }
    }
}
