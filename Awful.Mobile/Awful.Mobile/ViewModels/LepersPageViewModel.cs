// <copyright file="LepersPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Entities.Bans;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Lepers View Model.
    /// </summary>
    public class LepersPageViewModel : AwfulViewModel
    {
        private BanActions banActions;
        private ITemplateHandler handler;
        private BanPage banPage = new BanPage();
        private AwfulAsyncCommand firstPageCommand;
        private AwfulAsyncCommand previousPageCommand;
        private AwfulAsyncCommand nextPageCommand;
        private AwfulAsyncCommand lastPageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="LepersPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public LepersPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, context)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Gets or sets the internal webview.
        /// </summary>
        public IAwfulWebview WebView { get; set; }

        /// <summary>
        /// Gets or sets the current state of the view.
        /// </summary>
        public BanPage BanPage
        {
            get
            {
                return this.banPage;
            }

            set
            {
                this.SetProperty(ref this.banPage, value);
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets the First Page Command.
        /// </summary>
        public AwfulAsyncCommand FirstPageCommand
        {
            get
            {
                return this.firstPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.BanPage != null)
                        {
                            await this.LoadLepersPage(1).ConfigureAwait(false);
                        }
                    },
                    () => !this.IsBusy && this.BanPage.CurrentPage > 1,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Previous Page Command.
        /// </summary>
        public AwfulAsyncCommand PreviousPageCommand
        {
            get
            {
                return this.previousPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.BanPage != null)
                        {
                            if (this.BanPage.CurrentPage - 1 >= 1)
                            {
                                await this.LoadLepersPage(this.BanPage.CurrentPage - 1).ConfigureAwait(false);
                            }
                        }
                    },
                    () => !this.IsBusy && this.BanPage.CurrentPage > 1,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Next Page Command.
        /// </summary>
        public AwfulAsyncCommand NextPageCommand
        {
            get
            {
                return this.nextPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.BanPage != null)
                        {
                            if (this.BanPage.CurrentPage + 1 <= this.BanPage.TotalPages)
                            {
                                await this.LoadLepersPage(this.BanPage.CurrentPage + 1).ConfigureAwait(false);
                            }
                        }
                    },
                    () => !this.IsBusy && this.BanPage.CurrentPage < this.BanPage.TotalPages,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Last Page Command.
        /// </summary>
        public AwfulAsyncCommand LastPageCommand
        {
            get
            {
                return this.lastPageCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.BanPage != null)
                        {
                            await this.LoadLepersPage(this.BanPage.TotalPages).ConfigureAwait(false);
                        }
                    },
                    () => !this.IsBusy && this.BanPage.CurrentPage < this.BanPage.TotalPages,
                    this.Error);
            }
        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            if (this.IsSignedIn)
            {
                this.banActions = new BanActions(this.Client, this.Context, this.handler);
                await this.LoadLepersPage().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loads Lepers page.
        /// </summary>
        /// <param name="page">Page to load.</param>
        /// <returns>Task.</returns>
        public async Task LoadLepersPage(int page = 1)
        {
            this.IsBusy = true;
            var defaults = await this.GenerateDefaultOptionsAsync().ConfigureAwait(false);
            this.BanPage = await this.banActions.GetBanPageAsync(page).ConfigureAwait(false);
            this.WebView.SetSource(this.banActions.RenderBanView(this.BanPage, defaults));
            this.IsBusy = false;
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.FirstPageCommand.RaiseCanExecuteChanged();
            this.PreviousPageCommand.RaiseCanExecuteChanged();
            this.NextPageCommand.RaiseCanExecuteChanged();
            this.LastPageCommand.RaiseCanExecuteChanged();

            base.RaiseCanExecuteChanged();
        }
    }
}
