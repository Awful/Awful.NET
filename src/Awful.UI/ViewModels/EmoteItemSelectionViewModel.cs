// <copyright file="EmoteItemSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.Smilies;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Tools;
using Force.DeepCloner;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Emote Item Selection View Model.
    /// </summary>
    public class EmoteItemSelectionViewModel : AwfulViewModel
    {
        private ThreadPostCreationActions threadPostCreationActions;
        private AsyncCommand<object>? textChangedCommand;
        private List<SmileGroup> items = new List<SmileGroup>();
        private List<SmileGroup> originalItems = new List<SmileGroup>();
        private AsyncCommand<Smile>? selectionCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoteItemSelectionViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public EmoteItemSelectionViewModel(IServiceProvider services)
            : base(services)
        {
            threadPostCreationActions = new ThreadPostCreationActions(Client);
        }

        /// <summary>
        /// Gets the selection command.
        /// </summary>
        public AsyncCommand<Smile> SelectionCommand
        {
            get
            {
                return selectionCommand ??= new AsyncCommand<Smile>(
                    (item) =>
                    {
                        //if (item != null && this.popup != null)
                        //{
                        //    this.popup.SetIsVisible(false, item);
                        //}

                        return Task.CompletedTask;
                    },
                    null,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the selection command.
        /// </summary>
        public AsyncCommand<object> TextChangedCommand
        {
            get
            {
                return textChangedCommand ??= new AsyncCommand<object>(
                    (item) =>
                    {
                        if (item is string search)
                        {
                            FilterList(search);
                        }
                        else
                        {
                            FilterList(string.Empty);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets or sets the SmileCategory Items.
        /// </summary>
        public List<SmileGroup> Items
        {
            get
            {
                return items;
            }

            set
            {
                SetProperty(ref items, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Filter Emotes, with text.
        /// </summary>
        /// <param name="text">Text to filter by.</param>
        public void FilterList(string text)
        {
            if (!originalItems.Any())
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                Items = originalItems.DeepClone();
                OnPropertyChanged(nameof(Items));
                return;
            }

            Items = originalItems.Select(n => new SmileGroup(n.Title, n.Where(n => n.Title.Contains(text)).ToList())).ToList();
            OnPropertyChanged(nameof(Items));
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();

            if (threadPostCreationActions != null)
            {
                IsBusy = true;
                Items.Clear();
                var smileCategoryList = await threadPostCreationActions.GetSmileListAsync().ConfigureAwait(false);

                foreach (var icon in smileCategoryList.SmileCategories)
                {
                    originalItems.Add(new SmileGroup(icon.Name, icon.SmileList));
                }

                Items = originalItems.DeepClone();

                IsBusy = false;
            }
        }
    }
}
