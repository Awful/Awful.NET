// <copyright file="BookmarksPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Bookmarks Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookmarksPage : BasePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksPage"/> class.
        /// </summary>
        public BookmarksPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<BookmarksPageViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            this.ThreadListCollection.SelectedItem = null;
            base.OnAppearing();
        }
    }
}
