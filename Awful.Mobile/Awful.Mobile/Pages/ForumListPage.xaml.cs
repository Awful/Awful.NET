// <copyright file="ForumListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Autofac;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Forum List Page View.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumListPage : BasePage
    {
        private MobileForumsListPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumListPage"/> class.
        /// </summary>
        public ForumListPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<MobileForumsListPageViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            // Resets collection view on page load so you can retap on item.
            this.ForumCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}
