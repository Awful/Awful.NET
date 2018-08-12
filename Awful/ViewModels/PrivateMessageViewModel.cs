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
using Awful.Parser.Models.Messages;
using static Awful.ViewModels.ThreadViewModel;
using Awful.Controls;

namespace Awful.ViewModels
{
    public class PrivateMessageViewModel : AwfulViewModel
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

        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
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
        private PrivateMessageManager _postManager;

        #endregion

        public void Init()
        {
            LoginUser();
            _postManager = new PrivateMessageManager(WebManager);
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

        public async Task LoadPrivateMessage()
        {
            IsLoading = true;
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("reset", null));
            var postresult = await _postManager.GetPrivateMessageAsync(Selected);
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", new Thread() { IsLoggedIn = false, Posts = new List<Post>() { postresult } } ));
            OnPropertyChanged("Selected");
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

        public void Reply()
        {

        }

        internal async Task HandleForumCommand(ForumCommand test)
        {
            switch (test.Type)
            {
                default:
                    break;
            }
        }
    }
}
