using Mazui.Core.Models.Users;
using Mazui.Database.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.ViewModels
{
    public class ShellViewModel : MazuiViewModel
    {
        #region Properties
        private UserAuth _user = default(UserAuth);

        private bool _isLoggedIn = default(bool);

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
            }
        }

        public UserAuth User
        {
            get { return _user; }
            set
            {
                Set(ref _user, value);
            }
        }
        #endregion

        #region Methods

        public bool HasLogin()
        {
            User = UserAuthDatabase.GetDefaultUser();
            return User != null;
        }

        public void LoginUser()
        {
            IsLoggedIn = HasLogin();
        }

        #endregion
    }
}
