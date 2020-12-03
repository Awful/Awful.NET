// <copyright file="DebugPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Threading.Tasks;
using Autofac;
using Awful.Core.Entities.PostIcons;
using Awful.Core.Entities.Web;
using Awful.Core.Exceptions;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.Pages;
using Awful.Mobile.Tools.Utilities;
using Awful.Mobile.Views;
using Awful.UI.Actions;
using Awful.UI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xamarin.Essentials;
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
            this.ThrowAsyncExceptionCommand = new AwfulAsyncCommand(this.ThrowAsyncDebugException, null, this);
        }

        /// <summary>,
        /// Gets the throw exception command.
        /// </summary>
        public AwfulCommand ThrowExceptionCommand
        {
            get
            {
                return new AwfulCommand(this.ThrowDebugException, this);
            }
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
                        var forum = await this.Context.Forums.FirstOrDefaultAsync(n => n.Id == 273);
                        var awfulForum = new AwfulForum(forum);
                        var postIcon = new PostIcon();
                        var view = new ForumPostIconSelectionView(awfulForum, postIcon, new ThreadPostCreationActions(this.Client));
                        this.Popup.SetContent(view, true);
                    }
                },
                    null,
                    this);
            }
        }

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
