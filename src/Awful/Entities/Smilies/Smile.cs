// <copyright file="Smile.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.Smilies
{
    /// <summary>
    /// Something Awful Smile (Emoji).
    /// </summary>
    public class Smile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Smile"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="category">Category.</param>
        /// <param name="imageEndpoint">Image Endpoint.</param>
        public Smile(string title, string category, string imageEndpoint)
        {
            this.Title = title;
            this.Category = category;
            this.ImageEndpoint = imageEndpoint;
            this.ImageLocation = Path.GetFileNameWithoutExtension(imageEndpoint);
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the image endpoint.
        /// </summary>
        public string ImageEndpoint { get; }

        /// <summary>
        /// Gets the image location.
        /// </summary>
        public string ImageLocation { get; }
    }
}
