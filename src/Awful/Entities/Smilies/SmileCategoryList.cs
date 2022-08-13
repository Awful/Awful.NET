// <copyright file="SmileCategoryList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.Smilies
{
    /// <summary>
    /// Smile Category List.
    /// </summary>
    public class SmileCategoryList : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmileCategoryList"/> class.
        /// </summary>
        /// <param name="list">List of <see cref="SmileCategory"/>.</param>
        public SmileCategoryList(List<SmileCategory> list)
        {
            this.SmileCategories = list;
        }

        /// <summary>
        /// Gets the Smile Categories.
        /// </summary>
        public List<SmileCategory> SmileCategories { get; }
    }
}
