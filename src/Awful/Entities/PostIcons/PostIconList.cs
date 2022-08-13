// <copyright file="PostIconList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.PostIcons
{
    /// <summary>
    /// Post Icon List.
    /// </summary>
    public class PostIconList : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostIconList"/> class.
        /// </summary>
        /// <param name="icons">List of <see cref="PostIcon"/>.</param>
        public PostIconList(List<PostIcon> icons)
        {
            this.Icons = icons;
        }

        /// <summary>
        /// Gets the post icons.
        /// </summary>
        public List<PostIcon> Icons { get; internal set; }
    }
}
