// <copyright file="ThreadReply.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.Posts;

namespace Awful.Core.Entities.Replies
{
    public class ThreadReply : SAItem
    {
        public ThreadReply(string message, string? formKey = null, string? formCookie = null, string bookmark = "", string quote = "", string previousPostsRaw = "", int threadId = 0, long editPostId = 0, IEnumerable<Post>? posts = null, bool parseUrl = false)
        {
            this.ThreadId = threadId;
            this.Message = message;
            this.PostId = editPostId;
            this.ForumPosts = posts?.ToList().AsReadOnly() ?? new List<Post>().AsReadOnly();
            this.PreviousPostsRaw = previousPostsRaw;
            this.Bookmark = bookmark;
            this.FormKey = formKey;
            this.FormCookie = formCookie;
            this.ParseUrl = parseUrl;
            this.Quote = quote;
        }

        public string Message { get; set; }

        public bool ParseUrl { get; private set; }

        public string? FormKey { get; private set; }

        public string? FormCookie { get; private set; }

        public string Quote { get; set; }

        public int ThreadId { get; private set; }

        public long PostId { get; private set; }

        public string PreviousPostsRaw { get; set; }

        public string Bookmark { get; set; }

        public IReadOnlyList<Post> ForumPosts { get; }
    }
}
