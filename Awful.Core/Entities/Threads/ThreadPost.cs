// <copyright file="ThreadPost.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Threads
{
    public class ThreadPost : SAItem
    {
        public bool IsArchived { get; internal set; }
        public string LoggedInUserName { get; internal set; }
        public bool IsLoggedIn { get; internal set; }
        public int ScrollToPost { get; internal set; }
        public string ScrollToPostString { get; internal set; }
        public int CurrentPage { get; internal set; }
        public int TotalPages { get; internal set; }
        public bool LastPage => this.CurrentPage >= this.TotalPages;
        public string Name { get; internal set; }
        public int ThreadId { get; internal set; }
        public int ForumId { get; internal set; }
        public List<Post> Posts { get; internal set; } = new List<Post>();
    }
}
