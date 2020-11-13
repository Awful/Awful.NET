// <copyright file="AwfulForumCategory.cs" company="Drastic Actions">
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
    public class AwfulForumCategory
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [Key]
        public int Id { get; set; }

        public int SortOrder { get; set; }

        public string Title { get; set; }

        public virtual List<AwfulForum> Forums { get; set; }
    }
}
