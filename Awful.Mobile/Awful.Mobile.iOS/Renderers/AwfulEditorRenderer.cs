// <copyright file="AwfulEditorRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.ComponentModel;
using Awful.Mobile.Controls;
using Awful.Mobile.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AwfulEditor), typeof(AwfulEditorRenderer))]

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace Awful.Mobile.iOS
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    /// <summary>
    /// Awful Editor Renderer.
    /// </summary>
    public class AwfulEditorRenderer : EditorRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulEditorRenderer"/> class.
        /// </summary>
        public AwfulEditorRenderer()
        {
        }

        /// <inheritdoc/>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.NewElement is AwfulEditor)
            {
                if (this.TextView != null)
                {
                    this.TextView.SelectionChanged += this.TextView_SelectionChanged;
                }
            }
            else
            {
                if (this.TextView != null)
                {
                    this.TextView.SelectionChanged -= this.TextView_SelectionChanged;
                }
            }
        }

        private void TextView_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.Element is AwfulEditor editor)
            {
                var start = this.TextView.GetOffsetFromPosition(this.TextView.BeginningOfDocument, this.TextView.SelectedTextRange.Start);
                var end = this.TextView.GetOffsetFromPosition(this.TextView.BeginningOfDocument, this.TextView.SelectedTextRange.End);
                editor.SelectedTextStart = (int)start;
                editor.SelectedTextEnd = (int)end;
                editor.SelectedTextLength = (int)(end - start);
                editor.SelectedText = this.TextView.TextInRange(this.TextView.SelectedTextRange);
            }
        }
    }
}
