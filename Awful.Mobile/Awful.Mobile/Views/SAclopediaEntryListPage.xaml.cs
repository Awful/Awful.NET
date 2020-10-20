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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SAclopediaEntryListPage : ContentPage
    {
        public SAclopediaEntryListPage()
        {
            this.InitializeComponent();
            this.BindingContext = App.Container.Resolve<SAclopediaEntryListViewModel>();
        }
    }
}