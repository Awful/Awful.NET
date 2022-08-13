// <copyright file="EndPoints.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.Utilities
{
    /// <summary>
    /// Something Awful URL Endpoints.
    /// </summary>
    public static class EndPoints
    {
        /// <summary>
        /// Default Threads/Posts Per Page.
        /// </summary>
        public const int DefaultNumberPerPage = 40;

        /// <summary>
        /// Query string constant for going to a new post in a given thread.
        /// </summary>
        public const string GotoNewPost = "&goto=newpost";

        /// <summary>
        /// Query string constant for how many posts to show per page.
        /// </summary>
        public const string PerPage = "&perpage={0}";

        /// <summary>
        /// URL for a user profile.
        /// First parameter: User ID.
        /// </summary>
        public const string UserProfile = BaseUrl + "member.php?action=getinfo&userid={0}&json=1";

        /// <summary>
        /// URL for
        /// First parameter: User ID.
        /// </summary>
        public const string UserRapSheet = BaseUrl + "banlist.php?userid={0}";

        /// <summary>
        /// URL for
        /// First parameter: User ID.
        /// </summary>
        public const string RapSheet = BaseUrl + "banlist.php?pagenumber={0}";

        /// <summary>
        /// Hardcoded URL for getting the Forum List dropdown, hardcoded to Main (ID 48).
        /// </summary>
        public const string ForumListPage = "https://forums.somethingawful.com/forumdisplay.php?forumid=48";

        /// <summary>
        /// URL for getting a given forum list.
        /// First parameter: Forum ID.
        /// </summary>
        public const string ForumPage = BaseUrl + "forumdisplay.php?forumid={0}&perpage={1}";

        /// <summary>
        /// Base URL for resetting the read state of a given thread.
        /// First parameter: Thread ID.
        /// </summary>
        public const string ResetSeen = "action=resetseen&threadid={0}&json=1";

        /// <summary>
        /// Base URL for "Show Thread" commands.
        /// </summary>
        public const string ShowThreadBase = BaseUrl + "showthread.php";

        /// <summary>
        /// URL for bookmarking threads.
        /// </summary>
        public const string Bookmark = BaseUrl + "bookmarkthreads.php";

        /// <summary>
        /// URL for the last read command in a given thread.
        /// First parameter: Post Index.
        /// Second parameter: Thread Id.
        /// </summary>
        public const string LastRead = ShowThreadBase + "?action=setseen&index={0}&threadid={1}";

        /// <summary>
        /// URL for the last lost command in a given thread.
        /// First parameter: Thread Id.
        /// Second parameter: Page Number.
        /// </summary>
        public const string LastPost = ShowThreadBase + "?threadid={0}&pagenumber={1}#lastpost";

        /// <summary>
        /// Go to new post.
        /// </summary>
        public const string GotoNewPostEndpoint = ShowThreadBase + "?threadid={0}" + EndPoints.GotoNewPost;

        /// <summary>
        /// URL for removing a bookmark.
        /// First Parameter: Thread Id.
        /// </summary>
        public const string RemoveBookmark = "json=1&action=remove&threadid={0}";

        /// <summary>
        /// URL for adding a bookmark.
        /// First Parameter: Thread Id.
        /// </summary>
        public const string AddBookmark = "json=1&action=add&threadid={0}";

        /// <summary>
        /// URL for creating a new thread.
        /// First Parameter: Forum Id.
        /// </summary>
        public const string NewThread = BaseUrl + "newthread.php?action=newthread&forumid={0}";

        /// <summary>
        /// URL for creating a new private message.
        /// </summary>
        public const string NewPrivateMessageBase = BaseUrl + "private.php?action=newmessage";

        /// <summary>
        /// URL for creating a new thread.
        /// </summary>
        public const string NewThreadBase = BaseUrl + "newthread.php";

        /// <summary>
        /// URL for creating a new reply.
        /// </summary>
        public const string NewReply = BaseUrl + "newreply.php";

        /// <summary>
        /// URL for editing a post.
        /// </summary>
        public const string EditPost = BaseUrl + "editpost.php";

        /// <summary>
        /// URL for crafting a new reply.
        /// First Parameter: Thread Id.
        /// </summary>
        public const string ReplyBase = NewReply + "?action=newreply&threadid={0}";

        /// <summary>
        /// URL for crafting a quoted post.
        /// First Parameter: Post Id.
        /// </summary>
        public const string QuoteBase = NewReply + "?action=newreply&postid={0}";

        /// <summary>
        /// URL for editing an existing post.
        /// First Parameter: Post Id.
        /// </summary>
        public const string EditBase = BaseUrl + "editpost.php?action=editpost&postid={0}";

        /// <summary>
        /// URL for getting a users given post history.
        /// First Parameter: User Id.
        /// </summary>
        public const string UserPostHistory = BaseUrl + "search.php?action=do_search_posthistory&userid={0}";

        /// <summary>
        /// URL for getting the list of private messages.
        /// </summary>
        public const string PrivateMessages = BaseUrl + "private.php";

        /// <summary>
        /// Query handle for going to a specific page.
        /// </summary>
        public const string PageNumber = "&pagenumber={0}";

        /// <summary>
        /// URL for getting a given thread.
        /// First Parameter: Thread Id.
        /// </summary>
        public const string ThreadPage = BaseUrl + "showthread.php?threadid={0}&perpage={1}";

        /// <summary>
        /// URL for the front page of Something Awful. Not Used.
        /// </summary>
        public const string FrontPage = "https://www.somethingawful.com";

        /// <summary>
        /// URL for the smile list.
        /// </summary>
        public const string SmileUrl = BaseUrl + "misc.php?action=showsmilies";

        /// <summary>
        /// URL for showing a single post.
        /// First Parameter: Post Id.
        /// </summary>
        public const string ShowPost = BaseUrl + "showthread.php?action=showpost&postid={0}";

        /// <summary>
        /// URL for a Users Control Panel.
        /// </summary>
        public const string UserCp = BaseUrl + "usercp.php?";

        /// <summary>
        /// URL for the Something Awful cookie.
        /// </summary>
        public const string CookieDomainUrl = "https://fake.forums.somethingawful.com";

        /// <summary>
        /// URL for logging in.
        /// </summary>
        public const string LoginUrl = "https://forums.somethingawful.com/account.php?";

        /// <summary>
        /// URL for SAclopedia.
        /// </summary>
        public const string SAclopediaBase = BaseUrl + "dictionary.php";

        /// <summary>
        /// URL for the base forums.
        /// </summary>
        public const string BaseUrl = "https://forums.somethingawful.com/";

        /// <summary>
        /// URL for searching the forums.
        /// </summary>
        public const string SearchUrl = BaseUrl + "query.php";

        /// <summary>
        /// URL for a users bookmarks.
        /// First Parameter: Posts Per Page.
        /// </summary>
        public const string BookmarksUrl = BaseUrl + "bookmarkthreads.php?perpage={0}&sortorder=desc&sortfield=";

        /// <summary>
        /// URL for the JSON list of forums.
        /// </summary>
        public const string IndexPageUrl = BaseUrl + "index.php?json=1";
    }
}