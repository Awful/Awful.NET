﻿using Mazui.Core.Managers;
using Mazui.Core.Models.Posts;
using Mazui.Core.Models.Threads;
using Mazui.Core.Models.Web;
using Mazui.Tools;
using Mazui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;

namespace Mazui.ViewModels
{
    public class ThreadViewModel : MazuiViewModel
    {
        #region Properties
        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private string _pageSelection;

        public string PageSelection
        {
            get { return _pageSelection; }
            set
            {
                Set(ref _pageSelection, value);
            }
        }
        private PostManager _postManager;
        #endregion
        
        public async Task Init()
        {
            if (WebManager == null)
            {
                await LoginUser();
            }

            _postManager = new PostManager(WebManager);
        }

        #region Methods
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (WebManager == null)
            {
                await LoginUser();
            }

            _postManager = new PostManager(WebManager);

            if (suspensionState.ContainsKey(nameof(Thread)))
            {
                Selected = JsonConvert.DeserializeObject<Thread>(suspensionState[nameof(Thread)]?.ToString());
            }
            else
            {
                var thread = JsonConvert.DeserializeObject<Thread>((string)parameter);
                if (thread == null) return;
                Selected = thread;
            }

            await LoadThread(false);
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                if (Selected != null)
                {
                    var newThread = Selected.Clone();
                    newThread.Html = null;
                    newThread.Posts = null;
                    state[nameof(Selected)] = JsonConvert.SerializeObject(newThread);
                }
                state[nameof(Thread)] = JsonConvert.SerializeObject(Selected);
            }
            return Task.CompletedTask;
        }

        public async Task LoadThread(bool goToPageOverride = false)
        {
            var result = await _postManager.GetThreadPostsAsync(Selected.Location, Selected.CurrentPage, Selected.HasBeenViewed, goToPageOverride);
            if (await CheckResult(result) == false) return;
            await SetupPosts(result);
        }

        public async Task NextPage()
        {
            if (Selected.CurrentPage >= Selected.TotalPages) return;
            Selected.CurrentPage++;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await LoadThread();
        }

        public async Task PreviousPage()
        {
            if (Selected.CurrentPage <= 0) return;
            Selected.CurrentPage--;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await LoadThread();
        }

        public async Task ReloadThread()
        {
            await LoadThread();
        }

        public async Task FirstThreadPage()
        {
            Selected.CurrentPage = 1;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public async Task LastThreadPage()
        {
            Selected.CurrentPage = Selected.TotalPages;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public async Task ChangeThreadPage()
        {
            int userInputPageNumber;
            try
            {
                userInputPageNumber = Convert.ToInt32(PageSelection);
            }
            catch (Exception)
            {
                // User entered invalid number, return.
                return;
            }

            if (userInputPageNumber < 1 || userInputPageNumber > Selected.TotalPages) return;
            Selected.CurrentPage = userInputPageNumber;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public void ReplyToThread()
        {

        }

        public async void AddRemoveBookmark()
        {

        }

        public async void AddRemoveNotificationTable()
        {

        }

        private async Task SetupPosts(Result result)
        {
            var errorMessage = "";
            try
            {
                var postresult = JsonConvert.DeserializeObject<ThreadPosts>(result.ResultJson);
                Selected.LoggedInUserName = postresult.ForumThread.LoggedInUserName;
                Selected.CurrentPage = postresult.ForumThread.CurrentPage;
                Selected.TotalPages = postresult.ForumThread.TotalPages;
                Selected.Posts = postresult.Posts;

                // If the user is the "Test" user, say they are not logged in (even though they are)
                if (Selected.LoggedInUserName == "Testy Susan")
                {
                    IsLoggedIn = false;
                }
                var posts = postresult.Posts;
                return;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            await ResultChecker.SendMessageDialogAsync($"Error parsing thread: {errorMessage}", false);
        }

        private async Task<bool> CheckResult(Result result)
        {
            var resultCheck = await ResultChecker.CheckPaywallOrSuccess(result);
            if (!resultCheck)
            {
                if (result.Type == typeof(Error).ToString())
                {
                    var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
                    if (error.IsPaywall)
                    {
                        Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(PaywallPage));
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}