// <copyright file="MobileForumThreadListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Pages;
using Awful.UI.Actions;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Forum Thread List Page View Model.
    /// </summary>
    public class MobileForumThreadListPageViewModel : ForumThreadListPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileForumThreadListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public MobileForumThreadListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, AwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <summary>
        /// Navigate to New Thread page.
        /// </summary>
        /// <param name="forum">Awful Forum.</param>
        /// <returns>Task.</returns>
        protected override async Task NavigateToNewThreadPageAsync(AwfulForum forum)
        {
            await this.Navigation.PushModalAsync(new NewThreadPage(forum)).ConfigureAwait(false);
        }

        /// <summary>
        /// Navigate to New Thread page.
        /// </summary>
        /// <param name="thread">Awful Thread.</param>
        /// <returns>Task.</returns>
        protected override async Task NavigateToThreadPageAsync(AwfulThread thread)
        {
            await this.Navigation.PushDetailPageAsync(new ForumThreadPage(thread)).ConfigureAwait(false);
        }
    }
}
