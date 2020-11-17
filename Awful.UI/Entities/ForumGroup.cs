// <copyright file="ForumGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.Core.Entities.JSON;
using Awful.Database.Entities;

namespace Awful.UI.Entities
{
    public class ForumGroup : List<Forum>
    {
        public string Title { get; private set; }

        public ForumGroup(string name, List<Forum> entries)
            : base(entries.OrderBy(y => y.SortOrder))
        {
            this.Title = name;
        }
    }
}
