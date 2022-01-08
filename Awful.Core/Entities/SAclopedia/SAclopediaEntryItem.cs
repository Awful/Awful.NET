// <copyright file="SAclopediaEntryItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Entry Item.
    /// </summary>
    public class SAclopediaEntryItem
    {
        public SAclopediaEntryItem(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        /// <summary>
        /// Gets the id of the entry item.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the title of the entry item.
        /// </summary>
        public string Title { get; }
    }
}
