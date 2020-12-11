// <copyright file="ForumThreadPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Forum Thread Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumThreadPage : BasePage
    {
        private MobileForumThreadPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadPage"/> class.
        /// </summary>
        /// <param name="thread">Awful Thread.</param>
        public ForumThreadPage(AwfulThread thread)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<MobileForumThreadPageViewModel>();
            this.vm.LoadWebview(this.AwfulWebView, this.vm.HandleDataFromJavascript);
            this.vm.LoadThread(thread);
        }
    }
}
