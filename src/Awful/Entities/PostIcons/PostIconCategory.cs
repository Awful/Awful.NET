// <copyright file="PostIconCategory.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.PostIcons
{
    /// <summary>
    /// Something Awful Post Icon Category.
    /// </summary>
    public class PostIconCategory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostIconCategory"/> class.
        /// </summary>
        /// <param name="category">The name of the category.</param>
        /// <param name="list">The list of post icons.</param>
        public PostIconCategory(string category, List<PostIcon> list)
        {
            this.List = list;
            this.Category = category;
        }

        /// <summary>
        /// Gets the list of post icons.
        /// </summary>
        public virtual ICollection<PostIcon> List { get; private set; }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Category { get; private set; }
    }
}
