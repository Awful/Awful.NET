// <copyright file="AwfulForum.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Awful.Core.Entities.JSON;
using Force.DeepCloner;

namespace Awful.Database.Entities
{
    /// <summary>
    /// Awful Forum.
    /// </summary>
    public class AwfulForum : Forum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulForum"/> class.
        /// </summary>
        public AwfulForum()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulForum"/> class.
        /// </summary>
        /// <param name="parent">API Thread.</param>
        public AwfulForum(Forum parent)
        {
            parent.DeepCloneTo(this);
        }

        /// <summary>
        /// Gets or sets the ForumCategoryId.
        /// </summary>
        public int ForumCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the ForumCategory.
        /// </summary>
        public virtual AwfulForumCategory ForumCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the forum is a favorite.
        /// </summary>
        public bool IsFavorited { get; set; }
    }
}
