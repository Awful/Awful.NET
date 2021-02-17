// <copyright file="EmoteItemSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Smilies;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Force.DeepCloner;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Emote Item Selection View Model.
    /// </summary>
    public class EmoteItemSelectionViewModel : AwfulViewModel
    {
        private IAwfulPopup popup;
        private ThreadPostCreationActions threadPostCreationActions;
        private AwfulAsyncCommand<object> textChangedCommand;
        private List<SmileGroup> items = new List<SmileGroup>();
        private List<SmileGroup> originalItems = new List<SmileGroup>();
        private AwfulAsyncCommand<Smile> selectionCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoteItemSelectionViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public EmoteItemSelectionViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.popup = popup;
        }

        /// <summary>
        /// Gets the selection command.
        /// </summary>
        public AwfulAsyncCommand<Smile> SelectionCommand
        {
            get
            {
                return this.selectionCommand ??= new AwfulAsyncCommand<Smile>(
                    (item) =>
                    {
                        if (item != null && this.popup != null)
                        {
                            this.popup.SetIsVisible(false, item);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the selection command.
        /// </summary>
        public AwfulAsyncCommand<object> TextChangedCommand
        {
            get
            {
                return this.textChangedCommand ??= new AwfulAsyncCommand<object>(
                    (item) =>
                    {
                        if (item is string search)
                        {
                            this.FilterList(search);
                        }
                        else
                        {
                            this.FilterList(string.Empty);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets or sets the SmileCategory Items.
        /// </summary>
        public List<SmileGroup> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.SetProperty(ref this.items, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Filter Emotes, with text.
        /// </summary>
        /// <param name="text">Text to filter by.</param>
        public void FilterList(string text)
        {
            if (!this.originalItems.Any())
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                this.Items = this.originalItems.DeepClone();
                this.OnPropertyChanged(nameof(this.Items));
                return;
            }

            this.Items = this.originalItems.Select(n => new SmileGroup(n.Title, n.Where(n => n.Title.Contains(text)).ToList())).ToList();
            this.OnPropertyChanged(nameof(this.Items));
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.threadPostCreationActions = new ThreadPostCreationActions(this.Client);
            if (this.threadPostCreationActions != null)
            {
                this.IsBusy = true;
                this.Items.Clear();
                var smileCategoryList = await this.threadPostCreationActions.GetSmileListAsync().ConfigureAwait(false);

                foreach (var icon in smileCategoryList.SmileCategories)
                {
                    this.originalItems.Add(new SmileGroup(icon.Name, icon.SmileList));
                }

                this.Items = this.originalItems.DeepClone();

                this.IsBusy = false;
            }
        }
    }
}
