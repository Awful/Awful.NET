// <copyright file="ForumThreadListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Forum Thread List.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumThreadListPage : BasePage
    {
        private ForumThreadListPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListPage"/> class.
        /// </summary>
        /// <param name="forum"><see cref="AwfulForum"/>.</param>
        public ForumThreadListPage(AwfulForum forum)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumThreadListPageViewModel>();
            this.vm.LoadForum(forum);
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            // Resets collection view on page load so you can retap on item.
            this.ThreadListCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}
