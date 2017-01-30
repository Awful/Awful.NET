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

        public void CheckLogin()
        {
            try
            {
                User = UserAuthDatabase.GetDefaultUser();
                if (User != null) IsLoggedIn = true;
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion
    }
}
