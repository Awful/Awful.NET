// <copyright file="SmileCategory.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.Smilies
{
    /// <summary>
    /// Something Awful Smile Category.
    /// </summary>
    public class SmileCategory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmileCategory"/> class.
        /// </summary>
        /// <param name="name">Name of smilies.</param>
        /// <param name="smileList">List of smilies.</param>
        public SmileCategory(string name, List<Smile> smileList)
        {
            this.SmileList = smileList;
            this.Name = name;
        }

        /// <summary>
        /// Gets the list of smilies.
        /// </summary>
        public List<Smile> SmileList { get; }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Name { get; }
    }
}
