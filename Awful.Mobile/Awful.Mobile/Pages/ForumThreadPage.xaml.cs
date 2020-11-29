// <copyright file="ForumThreadPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Forum Thread Page.
    /// </summary>
    public partial class ForumThreadPage : BasePage
    {
        private ForumThreadPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadPage"/> class.
        /// </summary>
        public ForumThreadPage(AwfulThread thread)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumThreadPageViewModel>();
            this.vm.LoadWebview(this.AwfulWebView);
            this.vm.LoadThread(thread);
        }
    }
}
