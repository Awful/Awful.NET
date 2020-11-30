﻿// <copyright file="ForumThreadListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Forum Thread List.
    /// </summary>
    public partial class ForumThreadListPage : BasePage
    {
        private ForumThreadListPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListPage"/> class.
        /// </summary>
        /// <param name="forum">Awful Forum.</param>
        public ForumThreadListPage(AwfulForum forum)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumThreadListPageViewModel>();
            this.vm.LoadForum(forum);
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            this.ThreadListCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}