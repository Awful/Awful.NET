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
    public class ThreadList : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadList"/> class.
        /// </summary>
        public ThreadList()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadList"/> class.
        /// </summary>
        /// <param name="threads">List of <see cref="Thread"/>.</param>
        public ThreadList(List<Thread> threads)
        {
            this.Threads = threads;
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets the threads.
        /// </summary>
        public List<Thread> Threads { get; internal set; } = new List<Thread>();
    }
}
