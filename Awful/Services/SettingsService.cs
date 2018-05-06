using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Tools;

namespace Awful.Services
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Awful.Tools.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new SettingsHelper();
        }

        public bool InfinitePageScrolling
        {
            get { return _helper.Read<bool>(nameof(InfinitePageScrolling), false); }
            set
            {
                _helper.Write(nameof(InfinitePageScrolling), value);
            }
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
                ChangeBackgroundStatus(value);
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

        public async void ChangeBackgroundStatus(bool value)
        {
            if (value)
            {
                //var task = await
                //        BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                //            BackgroundTaskUtils.BackgroundTaskName,
                //            new TimeTrigger(15, false),
                //            null);
            }
            else
            {
               // BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
            }
        }

        public delegate void ChangedAppTheme();

        public event ChangedAppTheme ChangedAppThemeHandler;
    }
}
