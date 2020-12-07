// <copyright file="AwfulEditor.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Xamarin.Forms;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Awful Editor.
    /// </summary>
    public class AwfulEditor : Editor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulEditor"/> class.
        /// </summary>
        public AwfulEditor()
        {
            this.Text = string.Empty;
        }

        public bool IsTextSelected => !string.IsNullOrEmpty(this.SelectedText);

        /// <summary>
        /// Gets or sets the selected text in a view.
        /// </summary>
        public string SelectedText { get; set; }

        /// <summary>
        /// Gets or sets the selected text start point.
        /// </summary>
        public int SelectedTextStart { get; set; }

        /// <summary>
        /// Gets or sets the selected text end point.
        /// </summary>
        public int SelectedTextEnd { get; set; }

        /// <summary>
        /// Gets or sets the selected text length.
        /// </summary>
        public int SelectedTextLength { get; set; }
    }
}
