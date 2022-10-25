// <copyright file="ForumGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.JSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.UI.Entities
{
    /// <summary>
    /// Forum Group.
    /// Used to group forums by category.
    /// </summary>
    public class ForumGroup : ObservableCollection<AwfulForum>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumGroup"/> class.
        /// </summary>
        /// <param name="forum">Main Forum.</param>
        /// <param name="entries">List of Subforums.</param>
        public ForumGroup(Forum forum, List<Forum> entries)
            : base(entries.OrderBy(y => y.SortOrder).Select(n => new AwfulForum(n)))
        {
            if (forum == null)
            {
                throw new ArgumentNullException(nameof(forum));
            }

            Title = forum.Title ?? "Missing Title";
            Id = forum.Id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumGroup"/> class.
        /// </summary>
        /// <param name="title">Fake Title.</param>
        /// <param name="id">Fake id.</param>
        /// <param name="entries">List of Subforums.</param>
        public ForumGroup(string title, int id, List<Forum> entries)
            : base(entries.OrderBy(y => y.SortOrder).Select(n => new AwfulForum(n)))
        {
            Title = title;
            Id = id;
        }

        /// <summary>
        /// Gets the id of the forum group.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name of the forum.
        /// </summary>
        public string Title { get; private set; }
    }
}
