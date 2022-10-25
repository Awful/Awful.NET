using Awful;
using Awful.Entities.JSON;
using Awful.Managers.JSON;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Main Forum Actions.
    /// </summary>
    public class IndexPageActions
    {
        private IDatabaseContext context;
        private IndexPageManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPageActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public IndexPageActions(AwfulClient client, IDatabaseContext context)
        {
            manager = new IndexPageManager(client);
            this.context = context;
        }

        /// <summary>
        /// Get the forums category list.
        /// </summary>
        /// <param name="forceReload">Force Reloading.</param>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>List of Awful Forum Categories.</returns>
        public async Task<List<Forum>> GetForumListAsync(bool forceReload, CancellationToken token = default)
        {
            var awfulCatList = await context.GetForumCategoriesAsync().ConfigureAwait(false);
            if (!awfulCatList.Any() || forceReload)
            {
                var indexPageSorted = await manager.GetSortedIndexPageAsync(token).ConfigureAwait(false);
                awfulCatList = indexPageSorted.ForumCategories;
                if (awfulCatList is not null)
                {
                    await context.AddOrUpdateForumCategoriesAsync(awfulCatList).ConfigureAwait(false);
                }
            }

            return awfulCatList ?? new List<Forum>();
        }
    }
}
