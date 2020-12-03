// <copyright file="ForumPostIconSelectionPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Autofac;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.Tools.Utilities;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    public partial class ForumPostIconSelectionView : ContentView
    {
        private ForumPostIconSelectionViewModel vm;

        public ForumPostIconSelectionView(AwfulForum forum, PostIcon icon, ThreadPostCreationActions actions)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<ForumPostIconSelectionViewModel>();
            this.vm.LoadPostIcon(forum, icon, actions);
            this.vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(this.vm);
        }
    }
}
