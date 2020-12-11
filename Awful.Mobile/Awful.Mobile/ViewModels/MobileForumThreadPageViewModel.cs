// <copyright file="MobileForumThreadPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.UI.Entities;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Webview;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.ViewModels
{
    /// <summary>
    /// Mobile Forum Thread Page view Model.
    /// </summary>
    public class MobileForumThreadPageViewModel : ForumThreadPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileForumThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="handler">Awful Handler.</param>
        /// <param name="context">Awful Context.</param>
        public MobileForumThreadPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, ITemplateHandler handler, IAwfulContext context)
            : base(navigation, error, handler, context)
        {
        }

        /// <summary>
        /// Gets the reply to thread command.
        /// </summary>
        public AwfulAsyncCommand ReplyToThreadCommand
        {
            get
            {
                return this.replyToThreadCommand ??= new AwfulAsyncCommand(
                    async () =>
                    {
                        if (this.ThreadPost != null)
            {
                            await this.Navigation.PushModalAsync(new ThreadReplyPage(this.Thread.ThreadId)).ConfigureAwait(false);
                        }
                    },
                    () => !this.IsBusy && !this.OnProbation,
                    this.Error);
            }
        }

        public void HandleDataFromJavascript(string data)
        {
            var json = JsonConvert.DeserializeObject<WebViewDataInterop>(data);
            switch (json.Type)
            {
                case "showPostMenu":
                    // TODO: Refactor into generic method.
                    // TODO: Place into action? Command?
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await App.Current.MainPage.DisplayActionSheet("Post Options", "Cancel", null, "Share", "Mark Read", "Quote Post").ConfigureAwait(false);
                        switch (result)
                        {
                            case "Share":
                                await Share.RequestAsync(new ShareTextRequest
                                {
                                    Uri = string.Format(CultureInfo.InvariantCulture, EndPoints.ShowPost, json.Id),
                                    Title = this.Title,
                                }).ConfigureAwait(false);
                                break;
                            case "Mark Read":
                                _ = Task.Run(async () => await this.MarkPostAsUnreadAsync(json.Id).ConfigureAwait(false)).ConfigureAwait(false);
                                break;
                            case "Quote Post":
                                if (!this.OnProbation)
                                {
                                    await this.Navigation.PushModalAsync(new ThreadReplyPage(this.Thread.ThreadId, json.Id, false)).ConfigureAwait(false);
                                }
                                break;
                        }
                    });
                    break;
                case "showUserMenu":
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var options = !this.CanPM ? new string[2] { "Profile", "Their Posts" } : new string[3] { "Profile", "PM", "Their Posts" };
                        var result = await App.Current.MainPage.DisplayActionSheet("User Options", "Cancel", null, options).ConfigureAwait(false);
                        switch (result)
                        {
                            case "Profile":
                                await this.Navigation.PushDetailPageAsync(new UserProfilePage(json.Id)).ConfigureAwait(false);
                                break;
                            case "PM":
                                await this.Navigation.PushModalAsync(new NewPrivateMessagePage(json.Text)).ConfigureAwait(false);
                                break;
                            case "Their Posts":
                                break;
                        }
                    });
                    break;
            }
        }
    }
}
