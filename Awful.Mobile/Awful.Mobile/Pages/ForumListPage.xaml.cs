// <copyright file="ForumListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumListPage"/> class.
        /// </summary>
        public ForumListPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<ForumsListPageViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            this.ForumCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}
