// <copyright file="MobileEmoteItemSelectionViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Emote Item Selection View Model.
    /// </summary>
    public class MobileEmoteItemSelectionViewModel : EmoteItemSelectionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileEmoteItemSelectionViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public MobileEmoteItemSelectionViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, AwfulContext context)
            : base(popup, navigation, error, context)
        {
        }
    }
}
