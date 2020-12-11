// <copyright file="MobilePostEditItemSelectionViewModel.cs" company="Drastic Actions">
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
    /// Mobile Post Edit Item Selection View Model.
    /// </summary>
    public class MobilePostEditItemSelectionViewModel : PostEditItemSelectionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobilePostEditItemSelectionViewModel"/> class.
        /// </summary>
        /// <param name="popup">Awful Popup.</param>
        public MobilePostEditItemSelectionViewModel(IAwfulPopup popup)
            : base(popup)
        {
        }
    }
}
