// <copyright file="NewPrivateMessagePage.xaml.cs" company="Drastic Actions">
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
    /// <summary>
    /// New Private Message Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPrivateMessagePage : BasePage
    {
        private NewPrivateMessagePageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPrivateMessagePage"/> class.
        /// </summary>
        public NewPrivateMessagePage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<NewPrivateMessagePageViewModel>();
            this.vm.Editor = this.AwfulEditor;
        }
    }
}