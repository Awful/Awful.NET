// <copyright file="ForumPostIconSelectionView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Autofac;
using Awful.Core.Entities.PostIcons;
using Awful.Database.Entities;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
using Awful.UI.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Forum post Icon Selection View.
    /// Used to select and add a post icon to a new thread.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumPostIconSelectionView : ContentView
    {
        private MobileForumPostIconSelectionViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostIconSelectionView"/> class.
        /// </summary>
        /// <param name="forum">AwfulForum.</param>
        /// <param name="icon">Post Icon.</param>
        public ForumPostIconSelectionView(AwfulForum forum, PostIcon icon)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<MobileForumPostIconSelectionViewModel>();
            this.vm.LoadPostIcon(forum, icon);
            this.vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(this.vm.Error);
        }
    }
}
