// <copyright file="SAclopediaGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.SAclopedia;

namespace Awful.UI.Entities
{
    /// <summary>
    /// SAclopedia Group.
    /// Used for grouping SAclopedia Entry Items.
    /// </summary>
    public class SAclopediaGroup : List<SAclopediaEntryItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaGroup"/> class.
        /// </summary>
        /// <param name="name">Name of the group.</param>
        /// <param name="entries">List of SAclopedia Entry Items.</param>
        public SAclopediaGroup(string name, List<SAclopediaEntryItem> entries)
            : base(entries)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the group.
        /// This will probably be the first letter.
        /// </summary>
        public string Name { get; private set; }
    }
}
