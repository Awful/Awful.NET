using Awful.Managers;
using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Models.Web;
using Awful.Tools;
using Awful.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Awful.Services;
using System.IO;
using Windows.UI.Xaml.Controls;
using Awful.Web;

namespace Awful.ViewModels
{
    public class ThreadViewModel : AwfulViewModel
    {
        #region Properties

        public WebView Web { get; set; }

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

        public void Init()
        {
            if (WebManager == null)
            {
                LoginUser();
            }

            if (_postManager == null) _postManager = new PostManager(WebManager);
        }

        public async Task LoadThread(bool goToPageOverride = false)
        {
            IsLoading = true;
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("reset", null));
            var result = await _postManager.GetThreadPostsAsync(Selected.Location, Selected.CurrentPage, Selected.HasBeenViewed, goToPageOverride);
            if (await CheckResult(result) == false) return;
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", JsonConvert.DeserializeObject<ThreadPosts>(result.ResultJson)));
            IsLoading = false;
        }

        public WebCommands WebCommands { get; set; }

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
                        NavigationService.Navigate(typeof(PaywallPage));
                    }
                    return false;
                }
            }

            return true;
        }

        private async Task<ThreadPosts> SetupPosts(Result result)
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
                return postresult;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            await ResultChecker.SendMessageDialogAsync($"Error parsing thread: {errorMessage}", false);
            return null;
        }

        internal async Task HandleForumCommand(ForumCommand test)
        {
            //throw new NotImplementedException();
        }
    }
}
