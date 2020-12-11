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
        public MobileForumThreadListPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <summary>
        /// Gets the new thread command.
        /// </summary>
        public AwfulAsyncCommand NewThreadCommand
        {
            get
            {
                return this.newThreadCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.forum != null)
                        {
                            await this.Navigation.PushModalAsync(new NewThreadPage(this.forum)).ConfigureAwait(false);
                        }
                    },
                    () => !this.IsBusy && !this.OnProbation,
                    this.Error);
            }
        }

        /// <summary>
        /// Gets the Selection Entry.
        /// </summary>
        public AwfulAsyncCommand<AwfulThread> SelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<AwfulThread>(
                    async (item) =>
                    {
                        if (item != null)
                        {
                            await this.Navigation.PushDetailPageAsync(new ForumThreadPage(item)).ConfigureAwait(false);
                        }
                    },
                    null,
                    this.Error);
            }
        }
    }
}
