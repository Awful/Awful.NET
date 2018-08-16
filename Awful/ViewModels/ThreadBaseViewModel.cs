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
using static Awful.ViewModels.ThreadViewModel;

namespace Awful.ViewModels
{
    public class ThreadBaseViewModel : AwfulViewModel
    {
        #region Properties

        public WebCommands WebCommands { get; set; }

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

        #endregion

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

        public class ThreadSettings
        {
            public bool InfinitePageScrolling { get; set; }

            public bool ShowEmbeddedGifv { get; set; }

            public bool ShowEmbeddedVideo { get; set; }

            public bool ShowEmbeddedTweets { get; set; }

            public bool AutoplayGif { get; set; }

            public Themes Theme { get; set; }
        }

        public async Task SetupWebView()
        {
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("setupWebview", GetForumThreadSettings()));
        }


        public async Task<bool> CheckResult(Result result)
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
    }
}
