// <copyright file="EmoteItemSelectionView.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Awful.UI.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Emote Item Selection View.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmoteItemSelectionView : ContentView
    {
        private MobileEmoteItemSelectionViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoteItemSelectionView"/> class.
        /// </summary>
        public EmoteItemSelectionView()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = App.Container.Resolve<MobileEmoteItemSelectionViewModel>();
            this.vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(this.vm.Error);
        }
    }
}
