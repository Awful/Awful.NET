// <copyright file="NewThread.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.PostIcons;

namespace Awful.Core.Entities.Threads
{
    /// <summary>
    /// Something Awful New Thread.
    /// </summary>
    public class NewThread : SAItem
    {
        public int ForumId { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public PostIcon PostIcon { get; set; } = new PostIcon();

        public string FormKey { get; set; }

        public string FormCookie { get; set; }

        public bool ParseUrl { get; set; }
    }
}
