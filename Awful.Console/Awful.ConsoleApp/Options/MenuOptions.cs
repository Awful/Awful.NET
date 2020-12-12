// <copyright file="MenuOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.ConsoleApp.Options
{
    /// <summary>
    /// Main Menu Options.
    /// </summary>
    public enum MainMenu
    {
        /// <summary>
        /// Local HTML Parsing.
        /// </summary>
        LocalParsing,

        /// <summary>
        /// Testing Template Handler.
        /// </summary>
        TemplateHandler,

        /// <summary>
        /// Testing Awful Managers.
        /// </summary>
        AwfulManager,
    }

    /// <summary>
    /// Awful Manager Options.
    /// </summary>
    public enum AwfulManagerOption
    {
        /// <summary>
        /// Bookmark manager.
        /// </summary>
        BookmarkManager,
    }

    /// <summary>
    /// Bookmark Manager Options.
    /// </summary>
    public enum BookmarkManagerOption
    {
        /// <summary>
        /// Add Bookmark.
        /// </summary>
        AddBookmark,

        /// <summary>
        /// Remove Bookmark.
        /// </summary>
        RemoveBookmark,

        /// <summary>
        /// List Bookmarks By Page.
        /// </summary>
        ListBookmarksByPage,

        /// <summary>
        /// List All Bookmarks.
        /// </summary>
        ListAllBookmarks,
    }

    /// <summary>
    /// Template Handler Options.
    /// </summary>
    public enum TemplateHandlerOption
    {
        /// <summary>
        /// Thread Post.
        /// </summary>
        ThreadPost,

        /// <summary>
        /// Ban.
        /// </summary>
        Ban,

        /// <summary>
        /// User Profile.
        /// </summary>
        UserProfile,

        /// <summary>
        /// SAclopedia.
        /// </summary>
        SAclopedia,

        /// <summary>
        /// Acknowledgements.
        /// </summary>
        Acknowledgements,
    }
}
