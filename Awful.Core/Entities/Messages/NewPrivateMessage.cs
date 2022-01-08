// <copyright file="NewPrivateMessage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.PostIcons;

namespace Awful.Core.Entities.Messages
{
    /// <summary>
    /// New Private Message.
    /// </summary>
    public class NewPrivateMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewPrivateMessage"/> class.
        /// </summary>
        /// <param name="icon"><see cref="PostIcon"/>.</param>
        /// <param name="title">Title.</param>
        /// <param name="reciever">Reciever Username.</param>
        /// <param name="body">Body of text.</param>
        public NewPrivateMessage(PostIcon icon, string title, string reciever, string body)
        {
            this.Icon = icon;
            this.Title = title;
            this.Receiver = reciever;
            this.Body = body;
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        public PostIcon Icon { get; }

        /// <summary>
        /// Gets the title of the post.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the Receiver of the post.
        /// </summary>
        public string Receiver { get; }

        /// <summary>
        /// Gets the body of the post.
        /// </summary>
        public string Body { get; }
    }
}
