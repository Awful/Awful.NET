// <copyright file="MobileSAclopediaEntryPageViewModel.cs" company="Drastic Actions">
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
    /// Mobile SAclopedia Entry Page View Model.
    /// </summary>
    public class MobileSAclopediaEntryPageViewModel : SAclopediaEntryPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileSAclopediaEntryPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileSAclopediaEntryPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context) : base(navigation, error, handler, context)
        {
        }
    }
}
