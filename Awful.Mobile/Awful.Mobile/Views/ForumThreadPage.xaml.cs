// <copyright file="ForumThreadPage.xaml.cs" company="Drastic Actions">
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
    /// Forum Thread Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("EntryId", "entryId")]
    [QueryProperty("PageNumber", "pageNumber")]
    [QueryProperty("SATitle", "title")]
    public partial class ForumThreadPage : AuthorizationPage
    {
        private ForumThreadViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadPage"/> class.
        /// </summary>
        public ForumThreadPage()
        {
            this.InitializeComponent();
            this.vm = App.Container.Resolve<ForumThreadViewModel>();
            this.vm.WebView = this.AwfulWebView;
            this.BindingContext = this.vm;
        }

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string EntryId { get; set; }

        /// <summary>
        /// Gets or sets the page Number.
        /// </summary>
        public string PageNumber { get; set; } = "1";

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
            if (!string.IsNullOrEmpty(this.SATitle))
            {
                this.vm.Title = this.SATitle;
            }

            await this.vm.LoadTemplate(Convert.ToInt32(this.EntryId, CultureInfo.InvariantCulture), Convert.ToInt32(this.PageNumber, CultureInfo.InvariantCulture)).ConfigureAwait(false);
        }
    }
}