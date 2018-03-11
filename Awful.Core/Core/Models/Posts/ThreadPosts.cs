using Awful.Models.Threads;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Models.Posts
{
    public class ThreadPosts
    {
        public Thread ForumThread { get; set; } = new Thread();

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
