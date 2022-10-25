// <copyright file="LepersViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Entities.Bans;
using Awful.UI.Actions;
using Awful.UI.Controls;
using Awful.UI.Tools;

namespace Awful.UI.ViewModels
{
    public class LepersViewModel : AwfulWebviewViewModel
    {
        private BanActions banActions;
        private BanPage? banPage;
        private AsyncCommand? firstPageCommand;
        private AsyncCommand? previousPageCommand;
        private AsyncCommand? nextPageCommand;
        private AsyncCommand? lastPageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="LepersViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public LepersViewModel(IAwfulWebview webview, Action<string>? callback, IServiceProvider services)
            : base(webview, callback, services)
        {
            banActions = new BanActions(Client, Templates);
        }

        /// <summary>
        /// Gets or sets the current state of the view.
        /// </summary>
        public BanPage? BanPage
        {
            get
            {
                return banPage;
            }

            set
            {
                SetProperty(ref banPage, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets the First Page Command.
        /// </summary>
        public AsyncCommand FirstPageCommand
        {
            get
            {
                return firstPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (BanPage != null)
                        {
                            await LoadLepersPage(1).ConfigureAwait(false);
                        }
                    },
                    () => !IsBusy && BanPage?.CurrentPage > 1,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Previous Page Command.
        /// </summary>
        public AsyncCommand PreviousPageCommand
        {
            get
            {
                return previousPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (BanPage != null)
                        {
                            if (BanPage.CurrentPage - 1 >= 1)
                            {
                                await this.LoadLepersPage(BanPage.CurrentPage - 1).ConfigureAwait(false);
                            }
                        }
                    },
                    () => !IsBusy && BanPage?.CurrentPage > 1,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Next Page Command.
        /// </summary>
        public AsyncCommand NextPageCommand
        {
            get
            {
                return nextPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (BanPage != null)
                        {
                            if (BanPage.CurrentPage + 1 <= BanPage.TotalPages)
                            {
                                await this.LoadLepersPage(BanPage.CurrentPage + 1).ConfigureAwait(false);
                            }
                        }
                    },
                    () => !IsBusy && BanPage?.CurrentPage < BanPage?.TotalPages,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the Last Page Command.
        /// </summary>
        public AsyncCommand LastPageCommand
        {
            get
            {
                return lastPageCommand ??= new AsyncCommand(
                    async () =>
                    {
                        if (BanPage != null)
                        {
                            await this.LoadLepersPage(BanPage.TotalPages).ConfigureAwait(false);
                        }
                    },
                    () => !IsBusy && BanPage?.CurrentPage < BanPage?.TotalPages,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        public override async Task OnLoad()
        {
            await base.OnLoad();

            if (IsSignedIn)
            {
                await LoadLepersPage().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loads Lepers page.
        /// </summary>
        /// <param name="page">Page to load.</param>
        /// <returns>Task.</returns>
        public async Task LoadLepersPage(int page = 1)
        {
            IsBusy = true;
            var defaults = await GenerateDefaultOptionsAsync().ConfigureAwait(false);
            BanPage = await banActions.GetBanPageAsync(page).ConfigureAwait(false);
            WebView.SetSource(banActions.RenderBanView(BanPage, defaults));
            IsBusy = false;
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            FirstPageCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
            LastPageCommand.RaiseCanExecuteChanged();

            base.RaiseCanExecuteChanged();
        }
    }
}
