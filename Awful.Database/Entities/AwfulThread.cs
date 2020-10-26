// <copyright file="AwfulThread.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Awful.Core.Entities.Threads;
using Force.DeepCloner;

namespace Awful.Database.Entities
{
    /// <summary>
    /// Awful Database Thread.
    /// </summary>
    public class AwfulThread : Thread
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulThread"/> class.
        /// </summary>
        public AwfulThread()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulThread"/> class.
        /// </summary>
        /// <param name="parent">API Thread.</param>
        public AwfulThread(Thread parent)
        {
            parent.DeepCloneTo(this);
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Forum Id.
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// Gets or sets the Sort Order.
        /// </summary>
        public int SortOrder { get; set; }
    }
}
