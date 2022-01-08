// <copyright file="SAItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Web;

namespace Awful.Core.Entities
{
    /// <summary>
    /// Something Awful Item.
    /// Base for other web entities.
    /// </summary>
    public class SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SAItem"/> class.
        /// </summary>
        public SAItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SAItem"/> class.
        /// </summary>
        /// <param name="result">SA Web Result.</param>
        public SAItem(Result? result)
        {
            this.Result = result;
        }

        /// <summary>
        /// Gets or sets the raw web result.
        /// </summary>
        public Result? Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether the result is set.
        /// </summary>
        public bool IsResultSet => this.Result != null;
    }
}
