// <copyright file="EmoteItemSelectionView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.Tools;
using Xamarin.Forms;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Emote Item Selection View.
    /// </summary>
    public partial class EmoteItemSelectionView : ContentView
    {
        private EmoteItemSelectionViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoteItemSelectionView"/> class.
        /// </summary>
        public EmoteItemSelectionView()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<EmoteItemSelectionViewModel>();
            this.vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(this.vm);
        }
    }
}
