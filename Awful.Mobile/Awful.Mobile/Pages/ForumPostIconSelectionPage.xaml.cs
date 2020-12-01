// <copyright file="ForumPostIconSelectionPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
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
        /// <param name="forum"><see cref="AwfulForum"/>.</param>
        /// <param name="icon"><see cref="PostIcon"/>.</param>
        /// <param name="actions"><see cref="ThreadPostCreationActions"/>.</param>
        public ForumPostIconSelectionPage(AwfulForum forum, PostIcon icon, ThreadPostCreationActions actions)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumPostIconSelectionViewModel>();
            this.vm.LoadPostIcon(forum, icon, actions);
        }
    }
}