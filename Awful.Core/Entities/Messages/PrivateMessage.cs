// <copyright file="PrivateMessage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Messages
{
    /// <summary>
    /// SA Private Message.
    /// </summary>
    public class PrivateMessage
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int PrivateMessageId { get; set; }

        /// <summary>
        /// Gets or sets the Post Icon.
        /// </summary>
        public PostIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the Image Icon Endpoint.
        /// </summary>
        public string ImageIconLocation { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the sender of the pm.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets the message endpoint.
        /// </summary>
        public string MessageEndpoint { get; set; }

        /// <summary>
        /// Gets the image icon endpoint.
        /// </summary>
        public string ImageIconEndpoint { get; internal set; }

        /// <summary>
        /// Gets the Status Image Icon Endpoint.
        /// </summary>
        public string StatusImageIconEndpoint { get; internal set; }

        /// <summary>
        /// Gets the Status Image Icon Location.
        /// </summary>
        public string StatusImageIconLocation { get; internal set; }
    }
}
