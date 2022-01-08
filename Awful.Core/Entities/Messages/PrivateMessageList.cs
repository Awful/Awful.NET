// <copyright file="PrivateMessageList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Core.Entities.Messages
{
    /// <summary>
    /// Private Message List.
    /// </summary>
    public class PrivateMessageList : SAItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateMessageList"/> class.
        /// </summary>
        /// <param name="list">List of <see cref="PrivateMessage"/>.</param>
        public PrivateMessageList(List<PrivateMessage> list)
        {
            this.PrivateMessages = list;
        }

        /// <summary>
        /// Gets a list of private messages.
        /// </summary>
        public List<PrivateMessage> PrivateMessages { get; }
    }
}
