// <copyright file="MobileThreadReplyPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Database.Context;
using Awful.Mobile.Views;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Thread Reply Page View Model.
    /// </summary>
    public class MobileThreadReplyPageViewModel : ThreadReplyPageViewModel
    {
        private AwfulAsyncCommand postReplyCommand;
        private IAwfulPopup popup;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileThreadReplyPageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileThreadReplyPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context) 
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
        /// Gets the post thread command.
        /// </summary>
        public AwfulAsyncCommand PostThreadCommand
        {
            get
            {
                return this.postReplyCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.reply != null)
                        {
                            var result = await this.SendPostAsync().ConfigureAwait(false);

                            if (result == null)
                            {
                                return;
                            }

                            if (result.IsSuccess)
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                                    await this.Navigation.RefreshPostPageAsync().ConfigureAwait(false);
                                });
                            }
                        }
                    },
                    () => this.CanPost,
                    this.Error);
            }
        }
    }
}
