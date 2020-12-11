// <copyright file="MobileSettingsPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.ViewModels;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Settings Page View Model.
    /// </summary>
    public class MobileSettingsPageViewModel : SettingsPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileSettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="platformProperties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileSettingsPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IPlatformProperties platformProperties, AwfulContext context) : base(navigation, error, platformProperties, context)
        {
        }
    }
}
