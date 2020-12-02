// <copyright file="SAclopediaCategoryList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

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
        public SAclopediaCategoryList(List<SAclopediaCategory> list)
        {
            this.SAclopediaCategories = list;
        }

        /// <summary>
        /// Gets a list of SAclopedia Categories.
        /// </summary>
        public List<SAclopediaCategory> SAclopediaCategories { get; internal set; }
    }
}
