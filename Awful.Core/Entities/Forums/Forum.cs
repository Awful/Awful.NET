namespace Awful.Core.Entities.Forums
{
    public class Forum
    {
        public Forum(int id, string forumName, int parentForumId = 0, string? parentForumName = null)
        {
            this.ForumId = id;
            this.ForumName = forumName;
            this.ParentForumId = parentForumId;
            this.ParentForumName = parentForumName ?? string.Empty;
        }

        /// <summary>
        /// Gets the parent forum name.
        /// </summary>
        public string ParentForumName { get; }

        /// <summary>
        /// Gets the parent forum id.
        /// </summary>
        public int ParentForumId { get; }

        /// <summary>
        /// Gets the forum name.
        /// </summary>
        public string ForumName { get; }

        /// <summary>
        /// Gets the forum id.
        /// </summary>
        public int ForumId { get; }
    }
}
