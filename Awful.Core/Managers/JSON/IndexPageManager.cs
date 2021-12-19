﻿// <copyright file="IndexPageManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Utilities;

namespace Awful.Core.Managers.JSON
{
    /// <summary>
    /// Manager for the Main Forums Page.
    /// </summary>
    public class IndexPageManager
    {
        private readonly AwfulClient webManager;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPageManager"/> class.
        /// </summary>
        /// <param name="webManager"><see cref="AwfulClient"/>.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public IndexPageManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
        }

        /// <summary>
        /// Get the index page.
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/>.</param>
        /// <returns>Index Page.</returns>
        public async Task<IndexPage> GetIndexPageAsync(CancellationToken token = default)
        {
            var result = await this.webManager.GetDataAsync(EndPoints.IndexPageUrl, true, token).ConfigureAwait(false);
            try
            {
                var item = JsonSerializer.Deserialize<IndexPage>(result.ResultText);
                item.Result = result;
                return item;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        /// <summary>
        /// Get the sorted index page forum list.
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/>.</param>
        /// <returns>List of Forums.</returns>
        public async Task<SortedIndexPage> GetSortedIndexPageAsync(CancellationToken token = default)
        {
            var result = await this.webManager.GetDataAsync(EndPoints.IndexPageUrl, true, token).ConfigureAwait(false);

            try
            {
                var data = JsonSerializer.Deserialize<IndexPage>(result.ResultText);
                data.Result = result;
                foreach (var forum in data.Forums)
                {
                    this.UpdateForumMetadata(forum);
                }

                // The forums API returns null values for forums you can't access.
                // So if we see a zero for the ID, don't add it to the list.
                var forums = data.Forums.SelectMany(n => this.Flatten(n)).ToList();
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
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }

        private IEnumerable<Forum> Flatten(Forum forum)
        {
            yield return forum;
            if (forum.SubForums != null)
            {
                var forums = forum.SubForums.Where(n => n.Id > 0 && !string.IsNullOrEmpty(n.Title));
                foreach (var child in forums)
                {
                    foreach (var descendant in this.Flatten(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        private void UpdateForumMetadata(Forum forum, Forum? parentForum = null, int? categoryId = null)
        {
            if (parentForum != null)
            {
                forum.ParentForumId = parentForum.Id;
            }

            if (categoryId != null)
            {
                forum.ParentCategoryId = categoryId;
            }
            else
            {
                categoryId = forum.Id;
            }

            if (forum.SubForums == null)
            {
                return;
            }

            foreach (var subForum in forum.SubForums.ToList())
            {
               if (subForum.Id > 0 && !string.IsNullOrEmpty(subForum.Title))
               {
                    this.UpdateForumMetadata(subForum, forum, categoryId);
               }
               else
               {
                    forum.SubForums.Remove(subForum);
               }
            }
        }
    }
}
