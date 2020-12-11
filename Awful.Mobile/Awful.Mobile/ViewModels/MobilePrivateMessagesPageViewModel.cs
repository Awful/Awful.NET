// <copyright file="MobilePrivateMessagesPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Private Messages Page View Model.
    /// </summary>
    public class MobilePrivateMessagesPageViewModel : PrivateMessagesPageViewModel
    {
        private AwfulAsyncCommand newPMCommand;
        private AwfulAsyncCommand<AwfulPM> selectionCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobilePrivateMessagesPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobilePrivateMessagesPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context) 
            : base(navigation, error, handler, context)
        {
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<AwfulPM> SelectionCommand
        {
            get
            {
                return this.selectionCommand ??= new AwfulAsyncCommand<AwfulPM>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            await this.Navigation.PushDetailPageAsync(new PrivateMessagePage(item)).ConfigureAwait(false);
                        }
                    },
                    (item) => this.CanPM,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the new pm command.
        /// </summary>
        public AwfulAsyncCommand NewPMCommand
        {
            get
            {
                return this.newPMCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        await this.Navigation.PushModalAsync(new NewPrivateMessagePage()).ConfigureAwait(false);
                    },
                    () => !this.IsBusy && this.CanPM,
                    this.Error);
            }
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            this.NewPMCommand.RaiseCanExecuteChanged();
        }
    }
}
