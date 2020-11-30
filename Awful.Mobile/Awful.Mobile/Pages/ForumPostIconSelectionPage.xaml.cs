// <copyright file="ForumPostIconSelectionPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    /// <summary>
    /// Forum Post Icon Selection Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumPostIconSelectionPage : BasePage
    {
        private ForumPostIconSelectionViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostIconSelectionPage"/> class.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="actions"></param>
        public ForumPostIconSelectionPage(AwfulForum forum, PostIcon icon, ThreadPostCreationActions actions)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumPostIconSelectionViewModel>();
            this.vm.LoadPostIcon(forum, icon, actions);
        }
    }
}