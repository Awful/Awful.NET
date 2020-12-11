// <copyright file="MobileThreadReplyPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Thread Reply Page View Model.
    /// </summary>
    public class MobileThreadReplyPageViewModel : ThreadReplyPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileThreadReplyPageViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileThreadReplyPageViewModel(IAwfulPopup popup, IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context) : base(popup, navigation, error, handler, context)
        {
        }
    }
}
