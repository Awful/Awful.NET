// <copyright file="Index.cshtml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Awful.Web.Pages
{
    /// <summary>
    /// Index Model.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly AwfulClient client;
        private readonly IndexPageManager manager;

        public IndexModel(AwfulClient client)
        {
            this.client = client;
            this.manager = new IndexPageManager(this.client);
        }

        public SortedIndexPage Page;

        public async Task OnGet()
        {
            this.Page = await manager.GetSortedIndexPageAsync().ConfigureAwait(false);
        }

        public IEnumerable<Forum> Flatten(Forum forum)
        {
            yield return forum;
            if (forum.SubForums != null)
            {
                foreach (var child in forum.SubForums)
                {
                    foreach (var descendant in this.Flatten(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
