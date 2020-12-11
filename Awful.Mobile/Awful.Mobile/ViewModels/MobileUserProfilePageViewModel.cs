// <copyright file="MobileUserProfilePageViewModel.cs" company="Drastic Actions">
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
    /// Mobile User Profile Page View Model.
    /// </summary>
    public class MobileUserProfilePageViewModel : UserProfilePageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileUserProfilePageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileUserProfilePageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, TemplateHandler handler, AwfulContext context) : base(navigation, error, handler, context)
        {
        }
    }
}
