using Mazui.Core.Managers;
using Mazui.Core.Tools;
using Mazui.Database.Functions;
using Mazui.Tools;
using Mazui.Tools.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Filters;

namespace Mazui.ViewModels
{
    public class LoginPageViewModel : MazuiViewModel
    {
        #region Properties
        string _password = string.Empty;
        public string Password { get { return _password; } set { Set(ref _password, value); } }

        string _username = string.Empty;
        public string Username { get { return _username; } set { Set(ref _username, value); } }

        private readonly AuthenticationManager _authenticationManager = new AuthenticationManager();
        #endregion

        #region Methods
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            if (WebManager == null)
            {
                await LoginUser();
            }
            IsLoading = false;
        }

        public async Task LogoutUser()
        {
            IsLoading = true;
            try
            {
                await _authenticationManager.LogoutAsync(WebManager.AuthenticationCookie);
                await RemoveUserCookies();
                await UserAuthDatabase.RemoveUser(User);
                IsLoggedIn = false;
            }
            catch (Exception ex)
            {
                await ResultChecker.SendMessageDialogAsync($"Failed to log user out: {ex.Message}", false);
            }
            IsLoading = false;
        }

        public async Task LoginUserWithPassword()
        {
            IsLoading = true;
            await RemoveUserCookies();
            var result = await _authenticationManager.AuthenticateAsync(Username, Password);

            if (!result.IsSuccess)
            {
                await ResultChecker.SendMessageDialogAsync(result.Error, false);
                IsLoading = false;
                return;
            }

            WebManager = new WebManager(result.AuthenticationCookie);

            var userManager = new UserManager(WebManager);

            // 0 gets us the default user.
            var userResult = await userManager.GetUserFromProfilePage(0);
            if (userResult == null)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to get user", false);
                IsLoading = false;
                return;
            }

            try
            {
                var newUser = new Core.Models.Users.UserAuth { AvatarLink = userResult.AvatarLink, IsDefaultUser = true, UserName = userResult.Username, CookiePath = Guid.NewGuid().ToString() };
                await UserAuthDatabase.AddOrUpdateUser(newUser);
                await CookieManager.SaveCookie(newUser.CookiePath + ".txt", result.AuthenticationCookie, new Uri(EndPoints.CookieDomainUrl));
            }
            catch (Exception ex)
            {
                await ResultChecker.SendMessageDialogAsync($"Failed to save user: {ex.Message}", false);
                IsLoading = false;
                return;
            }

            IsLoading = false;
            IsLoggedIn = true;
            Views.Shell.Instance.ViewModel.IsLoggedIn = true;

            NavigationService.Navigate(typeof(Views.MainPage));
        }

        private async Task RemoveUserCookies()
        {
            var filter = new HttpBaseProtocolFilter();
            var cookieManager = filter.CookieManager;
            foreach (var cookie in cookieManager.GetCookies(new Uri("http://fake.forums.somethingawful.com")))
            {
                cookieManager.DeleteCookie(cookie);
            }
        }
        #endregion
    }
}
