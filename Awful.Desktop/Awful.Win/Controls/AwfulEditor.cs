// <copyright file="AwfulEditor.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Utilities;
using Awful.UI.Interfaces;
using Microsoft.UI.Xaml.Controls;

namespace Awful.Win.Controls
{
    /// <summary>
    /// Awful Editor.
    /// </summary>
    public class AwfulEditor : TextBox, IAwfulEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulEditor"/> class.
        /// </summary>
        public AwfulEditor()
        {
            this.Text = string.Empty;
        }

        /// <inheritdoc/>
        public bool IsTextSelected => !string.IsNullOrEmpty(this.SelectedText);

        /// <inheritdoc/>
        public int SelectedTextStart { get => this.SelectionStart; set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public int SelectedTextEnd { get => this.SelectionStart + this.SelectionLength; set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public int SelectedTextLength { get => this.SelectionLength; set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        void IAwfulEditor.Focus()
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => this.Focus(Microsoft.UI.Xaml.FocusState.Programmatic));
        }

        /// <inheritdoc/>
        public void UpdateText(string content)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
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
    }
}
