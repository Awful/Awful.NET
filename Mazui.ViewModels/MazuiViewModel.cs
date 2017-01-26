﻿using Mazui.Core.Managers;
using Mazui.Core.Models.Users;
using Mazui.Tools.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Mazui.ViewModels
{
    public class MazuiViewModel : ViewModelBase
    {
        private bool _isLoggedIn = default(bool);

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
            }
        }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private UserAuth _user = default(UserAuth);

        public UserAuth User
        {
            get { return _user; }
            set
            {
                Set(ref _user, value);
            }
        }

        public WebManager WebManager { get; set; }

        public async Task LoginUser()
        {
            var auth = await UserHandler.GetDefaultAuthWebManager();
            User = auth.User;
            IsLoggedIn = auth.IsLoggedIn;
            WebManager = auth.WebManager;
        }
    }
}