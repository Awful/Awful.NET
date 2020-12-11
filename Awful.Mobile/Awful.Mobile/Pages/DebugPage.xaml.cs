// <copyright file="DebugPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Debug Page. Used for testing out views.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DebugPage : BasePage
    {
        private DebugPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugPage"/> class.
        /// </summary>
        public DebugPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<DebugPageViewModel>();
            //this.vm.AwfulEditor = this.AwfulEditor;
        }
    }
}
