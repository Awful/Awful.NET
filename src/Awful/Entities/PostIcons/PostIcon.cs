// <copyright file="PostIcon.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Entities.PostIcons
{
    /// <summary>
    /// Something Awful Post Icon.
    /// </summary>
    public class PostIcon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostIcon"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="imageEndpoint">Image Endpoint.</param>
        /// <param name="title">Title.</param>
        public PostIcon(int id, string imageEndpoint, string title)
        {
            this.Id = id;
            this.ImageEndpoint = imageEndpoint;
            this.Title = title;
            this.ImageLocation = Path.GetFileNameWithoutExtension(this.ImageEndpoint);
        }

        /// <summary>
        /// Gets the id of the post icon.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Getsthe image icon endpoint.
        /// </summary>
        public string ImageEndpoint { get; }

        /// <summary>
        /// Gets the title of the post icon.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the image location of the icon.
        /// </summary>
        public string ImageLocation { get; }
    }
}
