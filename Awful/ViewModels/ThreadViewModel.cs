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

        private bool _isPageLoaded = default(bool);

        public bool IsPageLoaded
        {
            get { return _isPageLoaded; }
            set
            {
                Set(ref _isPageLoaded, value);
            }
        }

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
            LoginUser();
            _postManager = new PostManager(WebManager);
        }

        public async Task AddRemoveBookmarkView()
        {
            await this.AddRemoveBookmark(Selected);
        }

        public async Task AddRemoveNotificationTable()
        {
            await this.AddRemoveNotification(Selected);
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
            await ReloadThread(true);
        }

        public async Task FirstThreadPage()
        {
            Selected.CurrentPage = 1;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await ReloadThread(true);
        }

        public async Task LastThreadPage()
        {
            Selected.CurrentPage = Selected.TotalPages;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await ReloadThread(true);
        }

        public async Task LoadThread(bool goToPageOverride = false)
        {
            var result = await _postManager.GetThreadPostsAsync(Selected.Location, Selected.CurrentPage, Selected.HasBeenViewed, goToPageOverride);
            if (await CheckResult(result) == false) return;
            var threadPosts = JsonConvert.DeserializeObject<ThreadPosts>(result.ResultJson);
            await SetupPosts(threadPosts);
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", threadPosts));
            OnPropertyChanged("Selected");
        }

        public async Task NextPage()
        {
            if (Selected.CurrentPage >= Selected.TotalPages) return;
            Selected.CurrentPage++;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await ReloadThread();
        }

        public async Task PreviousPage()
        {
            if (Selected.CurrentPage <= 0) return;
            Selected.CurrentPage--;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await ReloadThread();

        }

        public async Task RefreshThread()
        {
            await ReloadThread();
        }

        public class ThreadSettings
        {
            public bool InfinitePageScrolling { get; set; }

            public bool ShowEmbeddedGifv { get; set; }

            public bool ShowEmbeddedVideo { get; set; }

            public bool ShowEmbeddedTweets { get; set; }

            public bool AutoplayGif { get; set; }
        }

        public ThreadSettings GetForumThreadSettings()
        {
            var settings = Awful.Services.SettingsService.Instance;
            ThreadSettings threadSettings = new ThreadSettings();
            threadSettings.InfinitePageScrolling = settings.InfinitePageScrolling;
            threadSettings.ShowEmbeddedGifv = settings.ShowEmbeddedGifv;
            threadSettings.ShowEmbeddedVideo = settings.ShowEmbeddedVideo;
            threadSettings.ShowEmbeddedTweets = settings.ShowEmbeddedTweets;
            threadSettings.AutoplayGif = settings.AutoplayGif;
            return threadSettings;
        }

        public async Task ReloadThread(bool goToPageOverride = false)
        {
            IsLoading = true;
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("reset", null));
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("setupWebview", GetForumThreadSettings()));
            await LoadThread(goToPageOverride);
            IsLoading = false;
        }

        public void ReplyToThread()
        {
            var reply = JsonConvert.SerializeObject(new ThreadReply()
            {
                Thread = new Thread()
                {
                    ForumId = Selected.ForumId,
                    ThreadId = Selected.ThreadId,
                    Name = Selected.Name,
                    CurrentPage = Selected.CurrentPage,
                    TotalPages = Selected.TotalPages
                }
            });
            // NavigationService.Navigate(typeof(ReplyPage), reply);
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

        private async Task SetupPosts(ThreadPosts postresult)
        {
            var errorMessage = "";
            try
            {
                Selected.LoggedInUserName = postresult.ForumThread.LoggedInUserName;
                Selected.CurrentPage = postresult.ForumThread.CurrentPage;
                Selected.TotalPages = postresult.ForumThread.TotalPages;
                Selected.Posts = postresult.Posts;
                // If the user is the "Test" user, say they are not logged in (even though they are)
                if (Selected.LoggedInUserName == "Testy Susan")
                {
                    IsLoggedIn = false;
                    Selected.IsLoggedIn = false;
                }

                var count = postresult.Posts.Count(node => !node.HasSeen);
                if (Selected.RepliesSinceLastOpened > 0)
                {
                    if ((Selected.RepliesSinceLastOpened - count < 0) || count == 0)
                    {
                        Selected.RepliesSinceLastOpened = 0;
                    }
                    else
                    {
                        Selected.RepliesSinceLastOpened -= count;
                    }
                }
                Selected.Name = postresult.ForumThread.Name;

                return;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            await ResultChecker.SendMessageDialogAsync($"Error parsing thread: {errorMessage}", false);
        }

        internal async Task HandleForumCommand(ForumCommand test)
        {
            switch(test.Type)
            {
                case "streaming":
                    if (Selected.CurrentPage >= Selected.TotalPages) return;
                    Selected.CurrentPage++;
                    Selected.ScrollToPost = 0;
                    Selected.ScrollToPostString = string.Empty;
                    await LoadThread();
                    break;
                case "loaded":
                    IsPageLoaded = true;
                    break;
                default:
                    break;
            }
        }
    }
}
