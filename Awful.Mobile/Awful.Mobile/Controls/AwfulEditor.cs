// <copyright file="AwfulEditor.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Core.Utilities;
using Awful.UI.Interfaces;
using Xamarin.Forms;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Awful Editor.
    /// </summary>
    public class AwfulEditor : Editor, IAwfulEditor
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

        public void UpdateText(string content)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // If user has selected text, replace it.
                // Or else, add it to whereever they have the cursor.
                if (this.IsTextSelected)
                {
                    this.Text = this.Text.ReplaceAt(this.SelectedTextStart, this.SelectedTextLength, content);
                }
                else
                {
                    this.Text = this.Text.Insert(this.SelectedTextStart, content);
                }
            });
        }

        void IAwfulEditor.Focus()
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => this.Focus());
        }
    }
}
