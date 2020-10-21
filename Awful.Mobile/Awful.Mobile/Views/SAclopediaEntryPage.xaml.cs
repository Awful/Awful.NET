// <copyright file="SAclopediaEntryPage.xaml.cs" company="Drastic Actions">
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
    [QueryProperty("EntryId", "entryId")]
    [QueryProperty("Title", "title")]
    public partial class SAclopediaEntryPage : ContentPage
    {
        private SAclopediaEntryViewModel vm = App.Container.Resolve<SAclopediaEntryViewModel>();

        public string EntryId { get; set; }

        public string Title { get; set; }

        public SAclopediaEntryPage()
        {
            this.InitializeComponent();
            this.vm.WebView = this.AwfulWebView;
            this.BindingContext = this.vm;
            //Task.Run(async () => await this.vm.LoadTemplate(new Core.Entities.SAclopedia.SAclopediaEntryItem() { Id = Convert.ToInt32(this.EntryId), Title = Uri.UnescapeDataString(this.Title) }).ConfigureAwait(false));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await this.vm.LoadTemplate(new Core.Entities.SAclopedia.SAclopediaEntryItem() { Id = Convert.ToInt32(this.EntryId), Title = Uri.UnescapeDataString(this.Title) }).ConfigureAwait(false);
        }
    }
}