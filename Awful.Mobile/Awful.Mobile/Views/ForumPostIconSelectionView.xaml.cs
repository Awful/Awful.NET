// <copyright file="ForumPostIconSelectionPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Autofac;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Entities;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    public partial class ForumPostIconSelectionView : ContentView
    {
        public ForumPostIconSelectionViewModel vm;

        public ForumPostIconSelectionView(Forms9Patch.PopupBase popup, AwfulForum forum, PostIcon icon, ThreadPostCreationActions actions)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumPostIconSelectionViewModel>();
            this.vm.LoadPostIcon(popup, forum, icon, actions);
        }
    }
}
