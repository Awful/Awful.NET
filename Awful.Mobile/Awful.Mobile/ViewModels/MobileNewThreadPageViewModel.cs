// <copyright file="MobileNewThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.Mobile.Views;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile New Thread Page View Model.
    /// </summary>
    public class MobileNewThreadPageViewModel : NewThreadPageViewModel
    {
        private IAwfulPopup popup;
        private AwfulAsyncCommand postThreadCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileNewThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileNewThreadPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, handler, context)
        {
            this.popup = popup;
        }

        /// <summary>
        /// Gets the options command.
        /// </summary>
        public AwfulAsyncCommand OpenOptionsCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    () =>
                    {
                        if (this.popup != null)
                        {
                            if (this.Editor != null)
                            {
                                var view = new PostEditItemSelectionView(this.Editor);
                                this.popup.SetContent(view, true);
                            }
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the SelectPostIcon Command.
        /// </summary>
        public AwfulAsyncCommand SelectPostIconCommand
        {
            get
            {
                return new AwfulAsyncCommand(
                    () =>
                    {
                        if (this.popup != null)
                        {
                            this.popup.SetContent(new ForumPostIconSelectionView(null, this.PostIcon), false, this.OnCloseModal);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the post thread command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return this.postThreadCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.newThread != null)
                        {

                            var result = await this.PostNewThreadAsync().ConfigureAwait(false);

                            // If we get a null result, we couldn't post at all. Ignore.
                            // TODO: It shouldn't ever return null anyway because of the Command Check.
                            // We probably don't need this check.
                            if (result == null)
                            {
                                return;
                            }

                            if (result.IsSuccess)
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                                    await this.Navigation.RefreshForumPageAsync().ConfigureAwait(false);
                                });
                            }
                        }
                    },
                    () => this.CanPost,
                    this.Error);
            }
        }

        /// <inheritdoc/>
        public override void OnCloseModal()
        {
            this.OnPropertyChanged(nameof(this.PostIcon));
            this.PostThreadCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.PostThreadCommand.RaiseCanExecuteChanged();
        }
    }
}
