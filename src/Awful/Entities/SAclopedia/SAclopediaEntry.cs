// <copyright file="SAclopediaEntry.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Entry.
    /// </summary>
    public class SAclopediaEntry : SAItem
    {
        public SAclopediaEntry(int id, string title, IEnumerable<SAclopediaPost> posts)
        {
            this.Id = id;
            this.Title = title;
            this.Posts = posts.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Gets the list of SAclopedia Posts.
        /// </summary>
        public IReadOnlyList<SAclopediaPost> Posts { get; }
    }
}
