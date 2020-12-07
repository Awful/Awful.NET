// <copyright file="IAwfulSearchPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Awful Search Page Interface.
    /// </summary>
    public interface IAwfulSearchPage
    {
        /// <summary>
        /// Search Bar Text Changed.
        /// </summary>
        event EventHandler<string> SearchBarTextChanged;

        /// <summary>
        /// On Search Bar Text Changed.
        /// </summary>
        /// <param name="text">Text that changed.</param>
        void OnSearchBarTextChanged(in string text);
    }
}
