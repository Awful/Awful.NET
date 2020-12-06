// <copyright file="SmileGroup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Awful.Core.Entities.Smilies;

namespace Awful.UI.Entities
{
    /// <summary>
    /// List of Smiles.
    /// </summary>
    public class SmileGroup : ObservableCollection<Smile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmileGroup"/> class.
        /// </summary>
        /// <param name="name">List of Smiles Group.</param>
        /// <param name="entries">List of Smiles.</param>
        public SmileGroup(string name, List<Smile> entries)
            : base(entries)
        {
            this.Title = name;
        }

        /// <summary>
        /// Gets the name of the smile group.
        /// </summary>
        public string Title { get; private set; }
    }
}
