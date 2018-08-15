using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.Tools;
using Awful.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using static Awful.ViewModels.ThreadViewModel;

namespace Awful.ViewModels
{
    public class PreviewViewModel : AwfulViewModel
    {
        public WebView Web { get; set; }

        public WebCommands WebCommands { get; set; }

        public async Task SetupWebView()
        {
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("setupWebview", GetForumThreadSettings()));
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

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        public async void LoadPost(Post post)
        {
            post.User = new Parser.Models.Users.User() { Roles = "", Username = User.UserName, AvatarLink = User.AvatarLink, DateJoined = DateTime.UtcNow };
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("reset", null));
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", new Thread() { IsLoggedIn = false, Posts = new List<Post>() { post } }));
        }
    }
}
