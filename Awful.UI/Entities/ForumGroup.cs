// <copyright file="ForumGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.JSON;

namespace Awful.UI.Entities
{
    public class ForumGroup : List<Forum>
    {
        public string Name { get; private set; }

        public ForumGroup(string name, List<Forum> entries)
            : base(entries)
        {
            this.Name = name;
        }
    }
}
