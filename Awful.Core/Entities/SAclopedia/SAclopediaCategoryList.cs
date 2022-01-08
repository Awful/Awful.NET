// <copyright file="SAclopediaCategoryList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Category List.
    /// </summary>
    public class SAclopediaCategoryList : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaCategoryList"/> class.
        /// </summary>
        /// <param name="list">List of <see cref="SAclopediaCategory"/>.</param>
        public SAclopediaCategoryList(IEnumerable<SAclopediaCategory> list)
        {
            this.SAclopediaCategories = list.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets a list of SAclopedia Categories.
        /// </summary>
        public IReadOnlyList<SAclopediaCategory> SAclopediaCategories { get; }
    }
}
