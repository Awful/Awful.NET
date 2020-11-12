// <copyright file="SAclopediaEntryPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// SAclopedia Entry Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("EntryId", "entryId")]
    [QueryProperty("SATitle", "title")]
    public partial class SAclopediaEntryPage : AuthorizationPage
    {
        private SAclopediaEntryViewModel vm = App.Container.Resolve<SAclopediaEntryViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SAclopediaEntryPage"/> class.
        /// </summary>
        public SAclopediaEntryPage()
        {
            this.InitializeComponent();
            this.vm.WebView = this.AwfulWebView;
            this.BindingContext = this.vm;
        }

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string EntryId { get; set; }

        /// <summary>
        /// Gets or sets the entry title.
        /// </summary>
        public string SATitle { get; set; }

        /// <summary>
        /// Shown On Appearing.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await this.vm.LoadTemplate(new Core.Entities.SAclopedia.SAclopediaEntryItem() { Id = Convert.ToInt32(this.EntryId, CultureInfo.InvariantCulture), Title = Uri.UnescapeDataString(this.SATitle) }).ConfigureAwait(false);
        }
    }
}