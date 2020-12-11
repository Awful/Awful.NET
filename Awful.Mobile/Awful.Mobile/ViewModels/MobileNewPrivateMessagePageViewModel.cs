// <copyright file="MobileNewPrivateMessagePageViewModel.cs" company="Drastic Actions">
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
    /// Mobile New Private Message Page View Model.
    /// </summary>
    public class MobileNewPrivateMessagePageViewModel : NewPrivateMessagePageViewModel
    {
        private IAwfulPopup popup;
        private AwfulAsyncCommand postPMCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileNewPrivateMessagePageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileNewPrivateMessagePageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
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
                            this.popup.SetContent(new ForumPostIconSelectionView(null, this.PostIcon), true, this.OnCloseModal);
                        }

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the post pm command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return this.postPMCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.newPrivateMessage != null)
                        {
                            var saItem = await this.SendPrivateMessageAsync().ConfigureAwait(false);

                            // If we get a null result, we couldn't post at all. Ignore.
                            // TODO: It shouldn't ever return null anyway because of the Command Check.
                            // We probably don't need this check.
                            if (saItem == null)
                            {
                                return;
                            }

                            if (saItem.IsResultSet && saItem.Result.IsSuccess)
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                                });
                            }
                        }
                    },
                    () => this.CanPostPm,
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
