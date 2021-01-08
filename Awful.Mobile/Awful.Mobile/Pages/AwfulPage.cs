// <copyright file="/Users/drasticactions/Developer/Projects/Awful.NET/Awful.Mobile/Awful.Mobile/Pages/AwfulPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Awful Page.
    /// </summary>
    public class AwfulPage : ContentPage
    {
        /// <inheritdoc/>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext is BaseViewModel vm)
            {
                vm.OnAppearingAsync().FireAndForgetSafeAsync(App.Container.Resolve<IAwfulErrorHandler>());
            }
        }
    }
}