// <copyright file="ThreadList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Thread List.
    /// </summary>
    public class ThreadList
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the threads.
        /// </summary>
        public List<Thread> Threads { get; } = new List<Thread>();
    }
}
