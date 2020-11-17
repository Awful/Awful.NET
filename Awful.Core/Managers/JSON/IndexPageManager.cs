// <copyright file="IndexPageManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Utilities;
using Newtonsoft.Json;

namespace Awful.Core.Managers.JSON
{
    /// <summary>
    /// Manager for the Main Forums Page.
    /// </summary>
    public class IndexPageManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPageManager"/> class.
        /// </summary>
        /// <param name="webManager"><see cref="AwfulClient"/>.</param>
        public IndexPageManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Get the index page.
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/>.</param>
        /// <returns>Index Page.</returns>
        public async Task<IndexPage> GetIndexPageAsync(CancellationToken token = default)
        {
            var result = await this.webManager.GetDataAsync(EndPoints.IndexPageUrl, token).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IndexPage>(result.ResultHtml);
        }

        /// <summary>
        /// Get the sorted index page forum list.
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/>.</param>
        /// <returns>List of Forums.</returns>
        public async Task<SortedIndexPage> GetSortedIndexPageAsync(CancellationToken token = default)
        {
            var result = await this.webManager.GetDataAsync(EndPoints.IndexPageUrl, token).ConfigureAwait(false);

            var data = JsonConvert.DeserializeObject<IndexPage>(result.ResultHtml);

            foreach (var forum in data.Forums)
            {
                this.UpdateForumMetadata(forum);
            }

            // The forums API returns null values for forums you can't access.
            // So if we see a zero for the ID, don't add it to the list.
            var forums = data.Forums.SelectMany(n => this.Flatten(n)).Where(n => n.Id != 0).ToList();
            for (int i = 0; i < forums.Count; i++)
            {
                forums[i].SortOrder = i + 1;
            }

            return new SortedIndexPage()
            {
                ForumCategories = data.Forums,
                Forums = forums.Where(n => n.HasThreads == true).ToList(),
                User = data.User,
                Stats = data.Stats,
            };
        }

        private IEnumerable<Forum> Flatten(Forum forum)
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

        private void UpdateForumMetadata(Forum forum, Forum parentForum = null)
        {
            if (parentForum != null)
            {
                forum.ParentForumId = parentForum.Id;
            }

            if (forum.SubForums == null)
            {
                return;
            }

            foreach (var subForum in forum.SubForums)
            {
                this.UpdateForumMetadata(subForum, forum);
            }
        }
    }
}
