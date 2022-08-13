// <copyright file="SAclopediaCategory.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.SAclopedia
{
    /// <summary>
    /// Category of SAclopedia.
    /// </summary>
    public class SAclopediaCategory
    {
        public SAclopediaCategory(int id, string letter)
        {
            this.Id = id;
            this.Letter = letter;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the letter.
        /// </summary>
        public string Letter { get; }
    }
}
