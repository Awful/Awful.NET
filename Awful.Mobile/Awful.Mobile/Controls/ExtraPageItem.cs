// <copyright file="ExtraPageItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Mobile.Pages;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Extra Page Item.
    /// Holds additional page data used for <see cref="ExtraPage"/>.
    /// </summary>
    public class ExtraPageItem
    {
        /// <summary>
        /// Gets or sets the type of page.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Font Awesome Glpyh icon for the page.
        /// To set, go to FontAwesome, find an icon, click on the glyph,
        /// and it will copy itself to your clipboard. You can then paste it
        /// in for this field and it should render.
        /// </summary>
        public string Glyph { get; set; }

        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description for the option.
        /// </summary>
        public string Description { get; set; }
    }
}
