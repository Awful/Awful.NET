// <copyright file="ThreadReplyPage.xaml.cs" company="Drastic Actions">
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
    /// Thread Reply Page.
    /// </summary>
    public partial class ThreadReplyPage : BasePage
    {
        private ThreadReplyPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyPage"/> class.
        /// </summary>
        public ThreadReplyPage(int threadId, int id = 0, bool isEdit = false)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ThreadReplyPageViewModel>();
            this.vm.Editor = this.AwfulEditor;
            this.vm.LoadThread(threadId, id, isEdit);
        }
    }
}
