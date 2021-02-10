// <copyright file="AwfulEditorRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awful.Mobile.Controls;
using Awful.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AwfulEditor), typeof(AwfulEditorRenderer))]

namespace Awful.Mobile.Droid.Renderers
{
    /// <summary>
    /// Awful Editor Renderer.
    /// </summary>
    public class AwfulEditorRenderer : EditorRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulEditorRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public AwfulEditorRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.NewElement is AwfulEditor)
            {
                if (this.Control != null)
                {
                    this.Control.LayoutChange += this.Control_LayoutChange;
                }
            }
            else
            {
                if (this.EditText != null)
                {
                    this.Control.LayoutChange -= this.Control_LayoutChange;
                }
            }
        }

        private void Control_LayoutChange(object sender, LayoutChangeEventArgs e)
        {
            if (this.Element is AwfulEditor editor && this.Control != null)
            {
                editor.SelectedTextStart = 0;
                editor.SelectedTextEnd = 0;
                editor.SelectedTextLength = 0;
                editor.SelectedText = string.Empty;

                int start = this.Control.SelectionStart;
                int end = this.Control.SelectionEnd;
                int selectionLength = end - start;
                if (end > 0 && end > start)
                {
                    string selection = this.Control.Text.Substring(start, selectionLength);
                    editor.SelectedTextStart = start;
                    editor.SelectedTextEnd = end;
                    editor.SelectedTextLength = selectionLength;
                    editor.SelectedText = selection;
                }
            }
        }
    }
}