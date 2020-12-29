// <copyright file="Thread.cshtml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Awful.Core.Entities.Threads;
using Awful.Core.Managers;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Awful.Web.Pages
{
    public class ThreadModel : PageModel
    {
        private readonly AwfulClient client;
        private readonly ThreadPostManager manager;
        private readonly HtmlParser parser;

        public ThreadModel(AwfulClient client, HtmlParser parser)
        {
            this.client = client;
            this.manager = new ThreadPostManager(this.client);
            this.parser = parser;
        }

        public ThreadPost ThreadPost;

        public async Task OnGet(int id, int pageNumber = 0, bool newestPost = false)
        {
            this.ThreadPost = await this.manager.GetThreadPostsAsync(id, pageNumber, newestPost).ConfigureAwait(false);
            foreach (var post in this.ThreadPost.Posts)
            {
                var threadHtml = await this.parser.ParseDocumentAsync(post.PostHtml).ConfigureAwait(false);
                foreach (var image in threadHtml.QuerySelectorAll("img"))
                {
                    image.SetAttribute("src", $"/proxy?file={image.GetAttribute("src")}");
                }

                post.PostHtml = threadHtml.DocumentElement.OuterHtml;
            }
        }
    }
}
