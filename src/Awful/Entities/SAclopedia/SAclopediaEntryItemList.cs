﻿// <copyright file="SAclopediaEntryItemList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.SAclopedia
{
    /// <summary>
    /// SAclopedia Entry Item List.
    /// </summary>
    public class SAclopediaEntryItemList : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryItemList"/> class.
        /// </summary>
        /// <param name="list">List of <see cref="SAclopediaEntryItem"/>.</param>
        public SAclopediaEntryItemList(IEnumerable<SAclopediaEntryItem> list)
        {
            this.SAclopediaEntryItems = list.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the SAclopedia Entry Items.
        /// </summary>
        public IReadOnlyList<SAclopediaEntryItem> SAclopediaEntryItems { get; }
    }
}
