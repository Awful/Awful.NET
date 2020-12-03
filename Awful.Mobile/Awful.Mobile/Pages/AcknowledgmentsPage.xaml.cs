// <copyright file="AcknowledgmentsPage.xaml.cs" company="Drastic Actions">
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
    /// Acknowledgements Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcknowledgmentsPage : BasePage
    {
        private AcknowledgmentsPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcknowledgmentsPage"/> class.
        /// </summary>
        public AcknowledgmentsPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<AcknowledgmentsPageViewModel>();
            this.vm.WebView = this.AwfulWebView;
        }
    }
}