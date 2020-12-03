// <copyright file="BasePage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Mobile.Tools.Utilities;
using Awful.Mobile.ViewModels;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Base Page. Used for setting up new pages.
    /// </summary>
    public partial class BasePage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class.
        /// </summary>
        public BasePage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext is MobileAwfulViewModel vm)
            {
                vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(vm);
            }
        }
    }
}
