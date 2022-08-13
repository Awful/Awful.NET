// <copyright file="ProbationItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.Bans
{
    /// <summary>
    /// Something Awful Probation Item.
    /// </summary>
    public class ProbationItem : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProbationItem"/> class.
        /// </summary>
        /// <param name="isUnderProbation">Is Under Probation.</param>
        /// <param name="probationUntil">Probation Until.</param>
        public ProbationItem(bool isUnderProbation, DateTime? probationUntil = null)
        {
            this.IsUnderProbation = isUnderProbation;
            this.ProbationUntil = probationUntil;
        }

        /// <summary>
        /// Gets the time until probation has concluded.
        /// </summary>
        public DateTime? ProbationUntil { get; }

        /// <summary>
        /// Gets a value indicating whether the current logged in user is under probation.
        /// </summary>
        public bool IsUnderProbation { get; }
    }
}