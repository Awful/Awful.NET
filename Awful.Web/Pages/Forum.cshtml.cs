// <copyright file="Forum.cshtml.cs" company="Drastic Actions">
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
    public class ForumModel : PageModel
    {
        private readonly AwfulClient client;
        private readonly ThreadListManager manager;
        private readonly HtmlParser parser;

        public ForumModel(AwfulClient client, HtmlParser parser)
        {
            this.client = client;
            this.parser = parser;
            this.manager = new ThreadListManager(this.client);
        }

        public ThreadList ThreadList;

        public async Task OnGet(int id, int pageNumber = 0)
        {
            this.ThreadList = await this.manager.GetForumThreadListAsync(id, pageNumber).ConfigureAwait(false);
        }
    }
}
