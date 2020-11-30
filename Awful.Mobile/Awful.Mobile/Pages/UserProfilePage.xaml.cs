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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : BasePage
    {
        private UserProfilePageViewModel vm;

        public UserProfilePage(long profileId)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<UserProfilePageViewModel>();
            this.vm.LoadProfile(profileId, this.AwfulWebView);
        }
    }
}