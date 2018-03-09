using Awful.Helpers;
using Awful.Managers;
using Awful.Models.Users;
using Awful.Parsers;
using Awful.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.ViewModels
{
    public class AwfulViewModel : Observable
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

        public void LoginUser()
        {
            var auth = UserAuthHandler.GetDefaultAuthWebManager();
            User = auth.User;
            IsLoggedIn = auth.IsLoggedIn;
            WebManager = auth.WebManager;
        }
    }
}
