using Awful.Parser.Managers;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
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
        private ThreadManager _postManager;
        #endregion

        public void Init()
        {
            LoginUser();
            _postManager = new ThreadManager(WebManager);
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
            await ReloadThread();
        }

        public async Task FirstThreadPage()
        {
            Selected.CurrentPage = 1;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await ReloadThread();
        }

        public async Task LastThreadPage()
        {
            Selected.CurrentPage = Selected.TotalPages;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await ReloadThread();
        }

        public async Task LoadThread(bool goToPageOverride = false)
        {
            var result = await _postManager.GetThreadAsync(Selected, goToPageOverride);
            await SetupPosts(result);
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", result));
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

            public Themes Theme { get; set; }
        }

        public enum Themes
        {
            Light,
            Dark,
            YOSPOS
        }

        public Themes GetTheme()
        {
            return ThemeSelectorService.Theme == Windows.UI.Xaml.ElementTheme.Light ? Themes.Light : Themes.Dark;
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
            threadSettings.Theme = GetTheme();
            return threadSettings;
        }

        public async Task SetupWebView()
        {
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("setupWebview", GetForumThreadSettings()));
        }

        public async Task ReloadThread(bool goToPageOverride = false)
        {
            IsLoading = true;
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("reset", null));
            await SetupWebView();
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

        private async Task SetupPosts(Thread postresult)
        {
            var errorMessage = "";
            try
            {
                //Selected.LoggedInUserName = postresult.LoggedInUserName;
                //Selected.CurrentPage = postresult.CurrentPage;
                //Selected.TotalPages = postresult.TotalPages;
                //Selected.Posts = postresult.Posts;
                // If the user is the "Test" user, say they are not logged in (even though they are)
                if (Selected.LoggedInUserName == "Testy Susan")
                {
                    IsLoggedIn = false;
                    Selected.IsLoggedIn = false;
                }

                var count = Selected.Posts.Count(node => !node.HasSeen);
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
                //Selected.Name = postresult.Name;
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
