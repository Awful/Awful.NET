using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Mazui.Services
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool ShowEmbeddedTweets
        {
            get { return _helper.Read<bool>(nameof(ShowEmbeddedTweets), true); }
            set
            {
                _helper.Write(nameof(ShowEmbeddedTweets), value);
            }
        }

        public bool AutoplayGif
        {
            get { return _helper.Read<bool>(nameof(AutoplayGif), true); }
            set
            {
                _helper.Write(nameof(AutoplayGif), value);
            }
        }

        public bool OpenThreadsInNewWindow
        {
            get { return _helper.Read<bool>(nameof(OpenThreadsInNewWindow), false); }
            set
            {
                _helper.Write(nameof(OpenThreadsInNewWindow), value);
            }
        }

        public bool ShowEmbeddedVideo
        {
            get { return _helper.Read<bool>(nameof(ShowEmbeddedVideo), true); }
            set
            {
                _helper.Write(nameof(ShowEmbeddedVideo), value);
            }
        }
        public bool ShowEmbeddedGifv
        {
            get { return _helper.Read<bool>(nameof(ShowEmbeddedVideo), true); }
            set
            {
                _helper.Write(nameof(ShowEmbeddedVideo), value);
            }
        }

        public bool BackgroundEnable
        {
            get { return _helper.Read<bool>(nameof(BackgroundEnable), false); }
            set
            {
                _helper.Write(nameof(BackgroundEnable), value);
                //ChangeBackgroundStatus(value);
            }
        }

        public bool TransparentThreadListBackground
        {
            get { return _helper.Read<bool>(nameof(TransparentThreadListBackground), false); }
            set
            {
                _helper.Write(nameof(TransparentThreadListBackground), value);
            }
        }

        public bool BookmarkBackground
        {
            get { return _helper.Read<bool>(nameof(BookmarkBackground), false); }
            set
            {
                _helper.Write(nameof(BookmarkBackground), value);
            }
        }

        public bool BookmarkNotifications
        {
            get { return _helper.Read<bool>(nameof(BookmarkNotifications), false); }
            set
            {
                _helper.Write(nameof(BookmarkNotifications), value);
            }
        }

        public bool ImgurSignedIn
        {
            get { return _helper.Read<bool>(nameof(ImgurSignedIn), false); }
            set
            {
                _helper.Write(nameof(ImgurSignedIn), value);
            }
        }

        public string ImgurUsername
        {
            get { return _helper.Read<string>(nameof(ImgurUsername), string.Empty); }
            set
            {
                _helper.Write(nameof(ImgurUsername), value);
            }
        }

        public DateTime LastRefresh
        {
            get { return _helper.Read<DateTime>(nameof(LastRefresh), DateTime.Now); }
            set
            {
                _helper.Write(nameof(LastRefresh), value);
            }
        }

        public delegate void ChangedAppTheme();

        public event ChangedAppTheme ChangedAppThemeHandler;

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = Application.Current.RequestedTheme;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value, true);
                if (ChangedAppThemeHandler != null) ChangedAppThemeHandler.Invoke();
            }
        }

        public bool IsFullScreen
        {
            get { return _helper.Read<bool>(nameof(IsFullScreen), false); }
            set
            {
                _helper.Write(nameof(IsFullScreen), value);
                Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }
    }
}
