// <copyright file="ThreadPostCreationActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful;
using Awful.Entities.PostIcons;
using Awful.Entities.Smilies;
using Awful.Managers;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Creation Post Actions.
    /// </summary>
    public class ThreadPostCreationActions
    {
        private SmileManager smileManager;
        private PostIconManager postIconManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostCreationActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        public ThreadPostCreationActions(AwfulClient client)
        {
            smileManager = new SmileManager(client);
            postIconManager = new PostIconManager(client);
        }

        /// <summary>
        /// Get Smile List.
        /// </summary>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>List of Smile Category.</returns>
        public async Task<SmileCategoryList> GetSmileListAsync(CancellationToken token = default)
        {
            return await smileManager.GetSmileListAsync(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Forum Post Icons for given Forum.
        /// </summary>
        /// <param name="forumId">Forum ID.</param>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>List of Forum Post Icons.</returns>
        public async Task<PostIconList> GetForumPostIconsAsync(int forumId, CancellationToken token = default)
        {
            return await postIconManager.GetForumPostIconsAsync(forumId, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Private Message Post Icons.
        /// </summary>
        /// <param name="token">Cancelation Token.</param>
        /// <returns>List of Private Message Post Icons.</returns>
        public async Task<PostIconList> GetPrivateMessagePostIconsAsync(CancellationToken token = default)
        {
            return await postIconManager.GetPrivateMessagePostIconsAsync(token).ConfigureAwait(false);
        }
    }
}
