// <copyright file="SAclopediaEntryItemList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Awful.Core.Entities.SAclopedia
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
        public SAclopediaEntryItemList(List<SAclopediaEntryItem> list)
        {
            this.SAclopediaEntryItems = list;
        }

        /// <summary>
        /// Gets the SAclopedia Entry Items.
        /// </summary>
        public List<SAclopediaEntryItem> SAclopediaEntryItems { get; internal set; }
    }
}
