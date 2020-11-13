// <copyright file="ForumGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.JSON;
using Awful.Database.Entities;

namespace Awful.UI.Entities
{
    public class ForumGroup : List<AwfulForum>
    {
        public string Title { get; private set; }

        public ForumGroup(string name, List<AwfulForum> entries)
            : base(entries)
        {
            this.Title = name;
        }
    }
}
