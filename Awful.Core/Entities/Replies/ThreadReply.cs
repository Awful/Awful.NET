// <copyright file="ThreadReply.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Awful.Core.Entities.Posts;
using Awful.Core.Entities.Threads;

namespace Awful.Core.Entities.Replies
{
    public class ThreadReply
    {
        public string Message { get; set; }

        public Thread Thread { get; private set; }

        public Post Post { get; private set; }

        public bool ParseUrl { get; private set; }

        public string FormKey { get; private set; }

        public string FormCookie { get; private set; }

        public string Quote { get; private set; }

        public string ThreadId { get; private set; }

        public long PostId { get; private set; }

        public string PreviousPostsRaw { get; set; }

        public string Bookmark { get; set; }

        public List<Post> ForumPosts { get; } = new List<Post>();

        public void MapMessage(string message)
        {
            this.Message = message;
            this.ParseUrl = true;
        }

        public void MapThreadInformation(string formKey, string formCookie, string quote, string threadId)
        {
            this.FormKey = formKey;
            this.FormCookie = formCookie;
            this.ThreadId = threadId;
            this.Quote = WebUtility.HtmlDecode(quote);
        }

        public void MapEditPostInformation(string quote, long postId, string bookmark)
        {
            this.Message = WebUtility.HtmlDecode(quote);
            this.PostId = postId;
            this.Bookmark = bookmark;
        }
    }
}
