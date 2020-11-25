// <copyright file="ForumThreadListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Autofac;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Views
{
    /// <summary>
    /// Forum Thread List Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("EntryId", "entryId")]
    [QueryProperty("SATitle", "title")]
    public partial class ForumThreadListPage : AuthorizationPage
    {
        private ForumThreadListViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumThreadListPage"/> class.
        /// </summary>
        public ForumThreadListPage()
        {
            this.InitializeComponent();
            this.vm = App.Container.Resolve<ForumThreadListViewModel>();
            this.BindingContext = this.vm;
        }

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string EntryId { get; set; } = "273";

        /// <summary>
        /// Gets or sets the entry title.
        /// </summary>
        public string SATitle { get; set; } = "General Bullshit";

        /// <summary>
        /// Shown On Appearing.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Title = this.SATitle;
            await this.vm.LoadThreadListAsync(Convert.ToInt32(this.EntryId), 0).ConfigureAwait(false);
        }
    }
}
