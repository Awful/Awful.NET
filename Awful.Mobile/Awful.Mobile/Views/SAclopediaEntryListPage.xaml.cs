// <copyright file="SAclopediaEntryListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// SAclopedia Entry List Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SAclopediaEntryListPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryListPage"/> class.
        /// </summary>
        public SAclopediaEntryListPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<SAclopediaEntryListViewModel>();
        }
    }
}