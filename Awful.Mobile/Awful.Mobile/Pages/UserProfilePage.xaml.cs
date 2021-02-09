// <copyright file="UserProfilePage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// User Profile Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : BasePage
    {
        private UserProfilePageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePage"/> class.
        /// </summary>
        /// <param name="profileId">User Profile Id.</param>
        public UserProfilePage(long profileId)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<UserProfilePageViewModel>();
            this.vm.LoadProfile(profileId, this.AwfulWebView);
        }
    }
}