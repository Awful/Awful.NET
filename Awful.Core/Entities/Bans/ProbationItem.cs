// <copyright file="ProbationItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Awful.Core.Entities.Bans
{
    /// <summary>
    /// Something Awful Probation Item.
    /// </summary>
    public class ProbationItem : SAItem
    {
        /// <summary>
        /// Gets or sets the time until probation has concluded.
        /// </summary>
        public DateTime ProbationUntil { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current logged in user is under probation.
        /// </summary>
        public bool IsUnderProbation { get; set; }
    }
}
