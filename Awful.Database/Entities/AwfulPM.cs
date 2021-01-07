// <copyright file="AwfulPM.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.Messages;
using Force.DeepCloner;

namespace Awful.Database.Entities
{
    /// <summary>
    /// Awful Private Message.
    /// </summary>
    public class AwfulPM : PrivateMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulPM"/> class.
        /// </summary>
        public AwfulPM()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulPM"/> class.
        /// </summary>
        /// <param name="parent">API Thread.</param>
        public AwfulPM(PrivateMessage parent)
        {
            parent.DeepCloneTo(this);
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Sort Order.
        /// </summary>
        public int SortOrder { get; set; }
    }
}
