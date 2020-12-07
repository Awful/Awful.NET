// <copyright file="PostEditItemSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Smilies;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.Views;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Forms9Patch;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Post Edit Items Seleciton View Model.
    /// </summary>
    public class PostEditItemSelectionViewModel : MobileAwfulViewModel
    {
        private AwfulEditor editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostEditItemSelectionViewModel"/> class.
        /// </summary>
        public PostEditItemSelectionViewModel()
        {
        }

        /// <summary>
        /// Gets the selection command.
        /// </summary>
        public AwfulAsyncCommand<EditPostItem> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<EditPostItem>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            switch (item.Type)
                            {
                                case EditPostItemType.Emotes:
                                    var emotePage = new EmoteItemSelectionView();
                                    this.Popup.SetContentWithParameter(emotePage, false, this.OnCloseModal);
                                    break;
                                case EditPostItemType.InsertImgur:
                                    break;
                                case EditPostItemType.InsertVideo:
                                    await this.AddTag(item, "video").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.InsertUrl:
                                    await this.AddUrl().ConfigureAwait(false);
                                    break;
                                case EditPostItemType.QuoteBlock:
                                    await this.AddTag(item, "quote").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.List:
                                    break;
                                case EditPostItemType.CodeBlock:
                                    break;
                                case EditPostItemType.PreserveSpace:
                                    await this.AddTag(item, "pre").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.Bold:
                                    await this.AddTag(item, "b").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.Italics:
                                    await this.AddTag(item, "i").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.Underline:
                                    await this.AddTag(item, "u").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.Strikeout:
                                    await this.AddTag(item, "s").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.SpoilerText:
                                    await this.AddTag(item, "spoiler").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.Superscript:
                                    await this.AddTag(item, "super").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.Subscript:
                                    await this.AddTag(item, "sub").ConfigureAwait(false);
                                    break;
                                case EditPostItemType.FixedWidth:
                                    await this.AddTag(item, "fixed").ConfigureAwait(false);
                                    break;
                            }
                        }
                    },
                    null,
                    this);
            }
        }

        /// <summary>
        /// Gets the list of Edit Post Items.
        /// </summary>
        public List<EditPostItem> EditPostItems
        {
            get
            {
            return new List<EditPostItem>()
            {
                new EditPostItem()
                {
                    Type = EditPostItemType.Emotes,
                    Glyph = "",
                    Title = "Emotes",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.InsertImgur,
                    Glyph = "",
                    Title = "Insert Imgur",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.InsertVideo,
                    Glyph = "",
                    Title = "Insert Video",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.InsertUrl,
                    Glyph = "",
                    Title = "Insert URL",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.QuoteBlock,
                    Glyph = "",
                    Title = "Quote Block",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.List,
                    Glyph = "",
                    Title = "List",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.CodeBlock,
                    Glyph = "",
                    Title = "Code Block",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.PreserveSpace,
                    Glyph = "",
                    Title = "Preserve Space",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.Bold,
                    Glyph = "",
                    Title = "Bold",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.Italics,
                    Glyph = "",
                    Title = "Italics",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.Underline,
                    Glyph = "",
                    Title = "Underline",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.Strikeout,
                    Glyph = "",
                    Title = "Strikeout",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.SpoilerText,
                    Glyph = "",
                    Title = "Spoiler Text",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.Superscript,
                    Glyph = "",
                    Title = "Superscript",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.Subscript,
                    Glyph = "",
                    Title = "Subscript",
                },
                new EditPostItem()
                {
                    Type = EditPostItemType.FixedWidth,
                    Glyph = "",
                    Title = "Fixed Width",
                },
            };
            }
        }

        /// <summary>
        /// Loads editor into VM.
        /// </summary>
        /// <param name="editor">SA Post Editor.</param>
        public void LoadEditor(AwfulEditor editor)
        {
            this.editor = editor;
        }

        private async Task AddUrl()
        {
            string uri = string.Empty;
            string innerText = string.Empty;
            if (this.editor.IsTextSelected)
            {
                Uri test;
                var success = Uri.TryCreate(this.editor.SelectedText, UriKind.Absolute, out test);
                if (success)
                {
                    uri = this.editor.SelectedText;
                }
                else
                {
                    innerText = this.editor.SelectedText;
                }
            }

            uri = await MobileAwfulViewModel.DisplayPromptAsync($"Insert URL", "Enter link to insert", "URL", uri, Keyboard.Url).ConfigureAwait(false);
            innerText = await MobileAwfulViewModel.DisplayPromptAsync($"Insert URL Inner Text", "Enter inner text", "Text", innerText).ConfigureAwait(false);

            if (string.IsNullOrEmpty(innerText))
            {
                this.SetTextInEditor($"[url]{uri}[/url]");
            }
            else
            {
                this.SetTextInEditor($"[url={uri}]{innerText}[/url]");
            }
        }

        private async Task AddTag(EditPostItem item, string tag)
        {
            if (this.editor.IsTextSelected)
            {
                this.SetTextInEditor($"[{tag}]{this.editor.SelectedText}[/{tag}]");
            }
            else
            {
                var result = await MobileAwfulViewModel.DisplayPromptAsync($"Insert Text: {item.Title}", "Enter text to insert").ConfigureAwait(false);
                this.SetTextInEditor($"[{tag}]{result}[/{tag}]");
            }
        }

        private void OnCloseModal(object response)
        {
            if (response is Smile smile)
            {
                this.SetTextInEditor(smile.Title);
            }
        }

        private void SetTextInEditor(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (this.editor != null)
                {
                    // If user has selected text, replace it.
                    // Or else, add it to whereever they have the cursor.
                    if (this.editor.IsTextSelected)
                    {
                        this.editor.Text = this.editor.Text.ReplaceAt(this.editor.SelectedTextStart, this.editor.SelectedTextLength, text);
                    }
                    else
                    {
                        this.editor.Text = this.editor.Text.Insert(this.editor.SelectedTextStart, text);
                    }
                }
            });

            this.Popup.SetIsVisible(false);
        }
    }
}
