// <copyright file="ProbationChangedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.Bans;

namespace Awful.Events
{
    /// <summary>
    /// Probation Changed Event Args.
    /// </summary>
    public class ProbationChangedEventArgs : EventArgs
    {
        private readonly ProbationItem? probation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbationChangedEventArgs"/> class.
        /// </summary>
        /// <param name="probation">Probation.</param>
        internal ProbationChangedEventArgs(ProbationItem? probation)
        {
            this.probation = probation;
        }

        /// <summary>
        /// Gets a value indicating whether the user is on probation.
        /// </summary>
        public bool OnProbation => this.probation != null;

        /// <summary>
        /// Gets the Probation Item.
        /// </summary>
        public ProbationItem? Probation => this.probation;
    }
}
