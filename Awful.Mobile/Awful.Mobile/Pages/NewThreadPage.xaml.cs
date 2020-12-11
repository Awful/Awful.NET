﻿// <copyright file="NewThreadPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// New Thread Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewThreadPage : BasePage
    {
        private MobileNewThreadPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewThreadPage"/> class.
        /// </summary>
        /// <param name="forum">Forum.</param>
        public NewThreadPage(AwfulForum forum)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<MobileNewThreadPageViewModel>();
            this.vm.Editor = this.AwfulEditor;
            this.vm.LoadForum(forum);
        }
    }
}