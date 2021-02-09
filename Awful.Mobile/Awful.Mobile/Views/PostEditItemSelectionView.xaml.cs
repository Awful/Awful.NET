// <copyright file="PostEditItemSelectionView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Post Edit Item Selection View.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostEditItemSelectionView : ContentView
    {
        private PostEditItemSelectionViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostEditItemSelectionView"/> class.
        /// </summary>
        /// <param name="editor">Post Editor.</param>
        public PostEditItemSelectionView(IAwfulEditor editor)
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<PostEditItemSelectionViewModel>();
            this.vm.LoadEditor(editor);
        }
    }
}