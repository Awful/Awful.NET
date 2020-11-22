// <copyright file="ForumGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Awful.Core.Entities.JSON;
using Awful.Database.Entities;

namespace Awful.UI.Entities
{
    public class ForumGroup : ObservableCollection<AwfulForum>
    {
        public int Id { get; private set; }
        public string Title { get; private set; }

        public ForumGroup(Forum forum, List<Forum> entries)
            : base(entries.OrderBy(y => y.SortOrder).Select(n => new AwfulForum(n)))
        {
            this.Title = forum.Title;
            this.Id = forum.Id;
        }
    }
}
