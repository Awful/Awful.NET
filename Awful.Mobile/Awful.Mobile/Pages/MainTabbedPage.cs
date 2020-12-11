// <copyright file="MainTabbedPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.Tools;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Main Tabbed Page.
    /// </summary>
    public class MainTabbedPage : Xamarin.Forms.TabbedPage
    {
        private MainTabbedPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainTabbedPage"/> class.
        /// </summary>
        public MainTabbedPage()
        {
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.BindingContext = this.vm = App.Container.Resolve<MainTabbedPageViewModel>();
            this.vm.SetupThemeAsync().ConfigureAwait(false);
            this.vm.LoadTabbedPage(this);
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext is Awful.UI.ViewModels.AwfulViewModel vm)
            {
                vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(this.vm.Error);
            }
        }
    }
}