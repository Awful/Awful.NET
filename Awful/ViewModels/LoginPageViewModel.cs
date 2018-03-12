using Awful.Managers;
using Awful.Tools;
using Awful.Database.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Filters;
using Awful.Models.Users;
using Newtonsoft.Json;
using Awful.Services;
using System.IO;

namespace Awful.ViewModels
{
    public class LoginPageViewModel : AwfulViewModel
    {
        #region Properties
        string _password = string.Empty;
        public string Password { get { return _password; } set { Set(ref _password, value); } }

        string _username = string.Empty;
        public string Username { get { return _username; } set { Set(ref _username, value); } }

        private AuthenticationManager _authenticationManager;
        #endregion

        #region Methods
        public void Load()
        {
            LoginUser();
            _authenticationManager = new AuthenticationManager(WebManager);
        }

        public async Task LogoutUser()
        {
            IsLoading = true;
            try
            {
                //await _authenticationManager.LogoutAsync(WebManager.AuthenticationCookie);
                await RemoveUserCookies();
                await UserAuthDatabase.RemoveUser(User);
                IsLoggedIn = false;
                App.ShellViewModel.PopulateNavItems();
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

            WebManager = new WebManager(result.AuthenticationCookieContainer);

            var userManager = new UserManager(WebManager);

            // 0 gets us the default user.
            var userResultJson = await userManager.GetUserFromProfilePageAsync(0);
            if (userResultJson == null)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to get user", false);
                IsLoading = false;
                return;
            }

            var userResult = JsonConvert.DeserializeObject<UserAuth>(userResultJson.ResultJson);

            try
            {
                var newUser = new UserAuth { AvatarLink = userResult.AvatarLink, IsDefaultUser = true, UserName = userResult.UserName, CookiePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Guid.NewGuid().ToString() + ".cookie") };
                await UserAuthDatabase.AddOrUpdateUser(newUser);
                CookieManager.SaveCookie(result.AuthenticationCookieContainer, newUser.CookiePath);
            }
            catch (Exception ex)
            {
                await ResultChecker.SendMessageDialogAsync($"Failed to save user: {ex.Message}", false);
                IsLoading = false;
                return;
            }

            IsLoading = false;
            IsLoggedIn = true;

            App.ShellViewModel.PopulateNavItems();
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
