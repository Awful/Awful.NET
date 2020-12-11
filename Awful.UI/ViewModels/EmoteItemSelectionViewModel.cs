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
using Xamarin.Forms;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Emote Item Selection View Model.
    /// </summary>
    public class EmoteItemSelectionViewModel : AwfulViewModel
    {
        private ThreadPostCreationActions threadPostCreationActions;
        private AwfulAsyncCommand<Smile> selectionCommand;
        private AwfulAsyncCommand<string> textChangedCommand;
        private ObservableCollection<SmileGroup> items = new ObservableCollection<SmileGroup>();
        private ObservableCollection<SmileGroup> originalItems = new ObservableCollection<SmileGroup>();
        private IAwfulPopup popup;

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
        public AwfulAsyncCommand<string> TextChangedCommand
        {
            get
            {
                return this.textChangedCommand ??= new AwfulAsyncCommand<string>(
                    (item) =>
                    {
                        var textItem = item.Trim();
                        if (string.IsNullOrEmpty(textItem))
                        {
                            this.Items = this.originalItems.DeepClone();
                            return Task.CompletedTask;
                        }

                        var items = this.originalItems.Where(n => n.Any(p => p.Title.Contains(textItem)));
                        this.Items.Clear();
                        foreach (var group in items)
                        {
                            var filteredList = group.Where(y => y.Title.Contains(textItem));
                            this.Items.Add(new SmileGroup(group.Title, filteredList.ToList()));
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
        public ObservableCollection<SmileGroup> Items
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
