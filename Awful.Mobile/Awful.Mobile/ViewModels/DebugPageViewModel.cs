// <copyright file="DebugPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.Mobile.Views;
using Awful.UI.Tools;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Debug Page View Model.
    /// </summary>
    public class DebugPageViewModel : MobileAwfulViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public DebugPageViewModel(AwfulContext context)
            : base(context)
        {
            this.OnProbation = true;
            this.OnProbationText = "TAKE A BREAK\nYou have been put on probation until Jun 25, 2025 13:50. You cannot post while\non probation. You might find out why you are on probation if you\ncheck the Leper's\nColony. If you read the fucking rules,\nmaybe this won't happen again!";
            this.ThrowAsyncExceptionCommand = new AwfulAsyncCommand(this.ThrowAsyncDebugException, null, this);
        }

        /// <summary>,
        /// Gets the throw exception command.
        /// </summary>
        public AwfulAsyncCommand MakePopupVisible
        {
            get
            {
                return new AwfulAsyncCommand(
                    async () =>
                {
                    if (this.Popup != null)
                    {
                        //var forum = await this.Context.Forums.FirstOrDefaultAsync(n => n.Id == 273);
                        //var awfulForum = new AwfulForum(forum);
                        //var postIcon = new PostIcon();
                        //var view = new ForumPostIconSelectionView(awfulForum, postIcon);

                        if (this.AwfulEditor != null)
                        {
                            var view = new PostEditItemSelectionView(this.AwfulEditor);
                            this.Popup.SetContent(view, true);
                        }
                    }
                },
                    null,
                    this);
            }
        }

        public Editor AwfulEditor { get; set; }

        public override async Task OnLoad()
        {
            throw new Exception("OH NO!");
        }

        /// <summary>
        /// Gets the throw exception command.
        /// </summary>
        public AwfulAsyncCommand ThrowAsyncExceptionCommand { get; internal set; }

        private async Task ThrowAsyncDebugException()
        {
            this.IsBusy = true;
            //this.OnPropertyChanged("ThrowAsyncExceptionCommand");
            await Task.Delay(2000).ConfigureAwait(false);
            throw new Exception("OH NO!");
        }

        private void ThrowDebugException()
        {
            this.IsBusy = true;
            throw new Exception("OH NO!");
        }
    }
}
