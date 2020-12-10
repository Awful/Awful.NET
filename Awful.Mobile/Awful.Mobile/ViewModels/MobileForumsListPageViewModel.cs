// <copyright file="MobileForumsListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Force.DeepCloner;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Awful Forums List View Model.
    /// </summary>
    public class MobileForumsListPageViewModel : ForumsListPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileForumsListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public MobileForumsListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, AwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <inheritdoc/>
        protected override async Task NavigateToForumThreadListPageAsync(AwfulForum forum)
        {
            await this.Navigation.PushPageAsync(new ForumThreadListPage(forum)).ConfigureAwait(false);
        }
    }
}
