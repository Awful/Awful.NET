using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.UI.Interfaces
{
    /// <summary>
    /// Awful Editor.
    /// </summary>
    public interface IAwfulEditor
    {
        /// <summary>
        /// Gets a value indicating whether the text is selected on the page.
        /// </summary>
        public bool IsTextSelected { get; }

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

        /// <summary>
        /// Gets or sets the text in the editor.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Update the text in the editor.
        /// </summary>
        /// <param name="content">Contents to update.</param>
        public void UpdateText(string content);

        /// <summary>
        /// Focus the editor.
        /// </summary>
        public void Focus();
    }
}
