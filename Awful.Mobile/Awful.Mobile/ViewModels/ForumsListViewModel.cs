// <copyright file="ForumsListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Tools;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.UI.Tools.Commands;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    public class ForumsListViewModel : AwfulViewModel
    {
        private IndexPageActions forumActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsListViewModel"/> class.
        /// </summary>
        /// <param name="properties">Awful Properties.</param>
        /// <param name="context">Awful Context.</param>
        public ForumsListViewModel(IPlatformProperties properties, AwfulContext context)
            : base(context)
        {
        }

        public async Task LoadForumsAsync(bool forceReload)
        {

        }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            this.forumActions = new IndexPageActions(this.Client, this.Context);
            await this.LoadForumsAsync(false).ConfigureAwait(false);
        }
    }
}
