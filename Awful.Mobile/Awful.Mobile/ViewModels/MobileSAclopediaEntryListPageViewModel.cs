// <copyright file="MobileSAclopediaEntryListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Core.Entities.SAclopedia;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile SAclopedia Entry List Page View Model.
    /// </summary>
    public class MobileSAclopediaEntryListPageViewModel : SAclopediaEntryListPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileSAclopediaEntryListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public MobileSAclopediaEntryListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context) : base(navigation, error, handler, context)
        {
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<SAclopediaEntryItem> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<SAclopediaEntryItem>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            await this.Navigation.PushDetailPageAsync(new SAclopediaEntryPage(item)).ConfigureAwait(false);
                        }
                    },
                    null,
                    this.Error);
            }
        }
    }
}
