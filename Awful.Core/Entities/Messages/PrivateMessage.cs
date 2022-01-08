// <copyright file="PrivateMessage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.Messages
{
    /// <summary>
    /// SA Private Message.
    /// </summary>
    public class PrivateMessage : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessage"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="statusImageIconEndpoint">Status Image Icon Endpoint.</param>
        /// <param name="title">Title.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="date">Date.</param>
        /// <param name="messageEndpoint">Message Endpoint.</param>
        /// <param name="imageIconEndpoint">Image Icon Endpoint, optional.</param>
        public PrivateMessage(int id, string statusImageIconEndpoint, string title, string sender, DateTime date, string messageEndpoint, string? imageIconEndpoint = null)
        {
            this.PrivateMessageId = id;
            this.ImageIconEndpoint = imageIconEndpoint ?? string.Empty;
            this.ImageIconLocation = Path.GetFileNameWithoutExtension(imageIconEndpoint);
            this.Title = title;
            this.Sender = sender;
            this.Date = date;
            this.MessageEndpoint = messageEndpoint;
            this.StatusImageIconEndpoint = statusImageIconEndpoint;
            this.StatusImageIconEndpoint = Path.GetFileNameWithoutExtension(statusImageIconEndpoint);
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        public int PrivateMessageId { get; }

        /// <summary>
        /// Gets the Image Icon Endpoint.
        /// </summary>
        public string? ImageIconLocation { get; }

        /// <summary>
        /// Gets the image icon endpoint.
        /// </summary>
        public string? ImageIconEndpoint { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Gets the sender of the pm.
        /// </summary>
        public string? Sender { get; }

        /// <summary>
        /// Gets the date.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets the message endpoint.
        /// </summary>
        public string? MessageEndpoint { get; }

        /// <summary>
        /// Gets the Status Image Icon Endpoint.
        /// </summary>
        public string? StatusImageIconEndpoint { get; }

        /// <summary>
        /// Gets the Status Image Icon Location.
        /// </summary>
        public string? StatusImageIconLocation { get; }
    }
}
