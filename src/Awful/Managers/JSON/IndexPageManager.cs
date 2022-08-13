// <copyright file="IndexPageManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.Json;
using Awful.Entities.JSON;
using Awful.Utilities;

namespace Awful.Managers.JSON
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
            var result = await this.webManager.GetDataAsync(EndPoints.IndexPageUrl, true, token).ConfigureAwait(false);
            try
            {
                var item = JsonSerializer.Deserialize<IndexPage>(result.ResultText);
                if (item is null)
                {
                    throw new Awful.Exceptions.AwfulParserException("Failed to parse IndexPage");
                }

                item.Result = result;
                return item;
            }
            catch (Exception ex)
            {
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
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
                if (data is null)
                {
                    throw new Awful.Exceptions.AwfulParserException("Failed to parse IndexPage");
                }

                data.Result = result;

                if (data.Forums is null)
                {
                    throw new Awful.Exceptions.AwfulParserException("Failed to parse Forums");
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
                throw new Awful.Exceptions.AwfulParserException(ex, new Awful.Entities.SAItem(result));
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
