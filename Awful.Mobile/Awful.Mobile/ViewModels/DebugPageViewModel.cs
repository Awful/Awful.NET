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
        Forms9Patch.FlyoutPopup popup;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugPageViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        public DebugPageViewModel(AwfulContext context)
            : base(context)
        {
            this.ThrowAsyncExceptionCommand = new AwfulAsyncCommand(this.ThrowAsyncDebugException, null, this);
            this.popup = new Forms9Patch.FlyoutPopup()
            {
                Alignment = Forms9Patch.FlyoutAlignment.End,
                Orientation = StackOrientation.Vertical,
                Content = new DefaultView(),
                Margin = 0,
                Padding = -5,
            };
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
                return new AwfulAsyncCommand(async () =>
                {
                    if (this.popup != null)
                    {
                        var forum = await this.Context.Forums.FirstOrDefaultAsync(n => n.Id == 273);
                        var awfulForum = new AwfulForum(forum);
                        var postIcon = new PostIcon();
                        var view = new ForumPostIconSelectionView(popup, awfulForum, postIcon, new ThreadPostCreationActions(this.Client));
                        this.popup.Content = view;
                        this.popup.IsVisible = true;
                        await view.vm.OnLoad();
                    }
                }, null, this);
            }
        }

        private bool CanExecuteButtonPress()
        {
            return !this.IsBusy;
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
