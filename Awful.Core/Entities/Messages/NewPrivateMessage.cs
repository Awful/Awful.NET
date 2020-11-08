// <copyright file="NewPrivateMessage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.PostIcons;

namespace Awful.Core.Entities.Messages
{
    /// <summary>
    /// New Private Message.
    /// </summary>
    public class NewPrivateMessage
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public PostIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Receiver of the post.
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// Gets or sets the body of the post.
        /// </summary>
        public string Body { get; set; }
    }
}
