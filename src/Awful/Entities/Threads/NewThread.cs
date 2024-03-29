﻿// <copyright file="NewThread.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.PostIcons;

namespace Awful.Entities.Threads
{
    /// <summary>
    /// Something Awful New Thread.
    /// </summary>
    public class NewThread : SAItem
    {
        public NewThread(string formKey, string formCookie)
        {
            this.FormKey = formKey;
            this.FormCookie = formCookie;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewThread"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="content">Content.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="formKey">Fork Key.</param>
        /// <param name="formCookie">Form Cookie.</param>
        /// <param name="parseUrl">Parse Url.</param>
        public NewThread(int id, string subject, string content, PostIcon icon, string formKey, string formCookie, bool parseUrl = true)
        {
            this.ForumId = id;
            this.Subject = subject;
            this.Content = content;
            this.PostIcon = icon;
            this.FormCookie = formCookie;
            this.FormKey = formKey;
            this.ParseUrl = parseUrl;
        }

        /// <summary>
        /// Gets the forum id.
        /// </summary>
        public int ForumId { get; }

        /// <summary>
        /// Gets or sets the post subject.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the post content.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the post icon.
        /// </summary>
        public PostIcon? PostIcon { get; set; }

        /// <summary>
        /// Gets the form key.
        /// </summary>
        public string FormKey { get; }

        /// <summary>
        /// Gets the form cookie.
        /// </summary>
        public string FormCookie { get; }

        /// <summary>
        /// Gets a value indicating whether to parse the url.
        /// </summary>
        public bool ParseUrl { get; }
    }
}
