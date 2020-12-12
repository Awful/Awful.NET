// <copyright file="MobileEmoteItemSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Core.Entities.Smilies;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Emote Item Selection View Model.
    /// </summary>
    public class MobileEmoteItemSelectionViewModel : EmoteItemSelectionViewModel
    {
        private IAwfulPopup popup;
        private AwfulAsyncCommand<Smile> selectionCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileEmoteItemSelectionViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public MobileEmoteItemSelectionViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
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
    }
}
