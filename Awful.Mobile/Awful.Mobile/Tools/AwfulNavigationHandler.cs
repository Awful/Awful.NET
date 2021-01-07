// <copyright file="AwfulNavigationHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Xamarin.Essentials;

namespace Awful.Mobile.Tools
{
    public class AwfulNavigationHandler : IAwfulNavigationHandler
    {
        public Task DisplayAlertAsync(string title, string message)
        {
            throw new NotImplementedException();
        }

        public Task<string> DisplayPromptAsync(string title, string message, string placeholder = "Text", string initialValue = "")
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(IAwfulContext context, IPlatformProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task PopModalAsync()
        {
            throw new NotImplementedException();
        }

        public Task PushDetailPageAsync(object page)
        {
            throw new NotImplementedException();
        }

        public Task PushModalAsync(object page)
        {
            throw new NotImplementedException();
        }

        public Task PushPageAsync(object page)
        {
            throw new NotImplementedException();
        }

        public Task RefreshForumPageAsync()
        {
            throw new NotImplementedException();
        }

        public Task RefreshPostPageAsync()
        {
            throw new NotImplementedException();
        }

        public void SetMainAppPage()
        {
            MainThread.BeginInvokeOnMainThread(() => App.Current.MainPage = new MainPage());
        }

        public Task SetMainAppPageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
