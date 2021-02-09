// <copyright file="ThreadReplyPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Thread Reply Page.
    /// </summary>
    ///
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThreadReplyPage : BasePage
    {
        private ThreadReplyPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadReplyPage"/> class.
        /// </summary>
        /// <param name="threadId">Thread Id.</param>
        /// <param name="id">Post Id.</param>
        /// <param name="isEdit">Is the post an edit.</param>
        public ThreadReplyPage(int threadId, int id = 0, bool isEdit = false)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ThreadReplyPageViewModel>();
            this.vm.Editor = this.AwfulEditor;
            this.vm.LoadThread(threadId, id, isEdit);
        }
    }
}
