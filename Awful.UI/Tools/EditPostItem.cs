// <copyright file="EditPostItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.UI.Tools
{
    /// <summary>
    /// Edit Post Item.
    /// </summary>
    public class EditPostItem
    {
        /// <summary>
        /// Gets or sets the type of command.
        /// </summary>
        public EditPostItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the Font Awesome Glpyh icon for the page.
        /// To set, go to FontAwesome, find an icon, click on the glyph,
        /// and it will copy itself to your clipboard. You can then paste it
        /// in for this field and it should render.
        /// </summary>
        public string Glyph { get; set; }

        /// <summary>
        /// Gets or sets the title of the command.
        /// </summary>
        public string Title { get; set; }
    }

    public enum EditPostItemType
    {
        Emotes,
        InsertImgur,
        InsertVideo,
        InsertUrl,
        QuoteBlock,
        List,
        CodeBlock,
        PreserveSpace,
        Bold,
        Italics,
        Underline,
        Strikeout,
        SpoilerText,
        Superscript,
        Subscript,
        FixedWidth
    }
}
