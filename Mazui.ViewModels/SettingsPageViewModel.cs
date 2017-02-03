using Mazui.Tools.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Mazui.ViewModels
{
    public class SettingsPageViewModel : MazuiViewModel
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : MazuiViewModel
    {
        readonly Services.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsService.Instance;
        }

        public bool UseOpenNewThreadInWindowButton
        {
            get { return _settings.OpenThreadsInNewWindow; }
            set { _settings.OpenThreadsInNewWindow = value; base.RaisePropertyChanged(); }
        }

        public bool UseShowEmbeddedGifvButton
        {
            get { return _settings.ShowEmbeddedGifv; }
            set { _settings.ShowEmbeddedGifv = value; base.RaisePropertyChanged(); }
        }

        public bool UseShowEmbeddedTweetsButton
        {
            get { return _settings.ShowEmbeddedTweets; }
            set { _settings.ShowEmbeddedTweets = value; base.RaisePropertyChanged(); }
        }

        public bool UseShowEmbeddedVideoButton
        {
            get { return _settings.ShowEmbeddedVideo; }
            set { _settings.ShowEmbeddedVideo = value; base.RaisePropertyChanged(); }
        }

        public bool AlwaysAutoplayGif
        {
            get { return _settings.AutoplayGif; }
            set { _settings.AutoplayGif = value; base.RaisePropertyChanged(); }
        }

        public bool UseBackgroundTask
        {
            get { return _settings.BackgroundEnable; }
            set { _settings.BackgroundEnable = value; base.RaisePropertyChanged(); }
        }

        public bool UseBackgroundBookmarkLiveTile
        {
            get { return _settings.BookmarkBackground; }
            set { _settings.BookmarkBackground = value; base.RaisePropertyChanged(); }
        }

        public bool UseTransparentThreadListBackground
        {
            get { return _settings.TransparentThreadListBackground; }
            set { _settings.TransparentThreadListBackground = value; base.RaisePropertyChanged(); }
        }

        public bool UseBookmarkBackgroundNotify
        {
            get { return _settings.BookmarkNotifications; }
            set { _settings.BookmarkNotifications = value; base.RaisePropertyChanged(); }
        }

        public bool ImgurSignedIn
        {
            get { return _settings.ImgurSignedIn; }
            set { _settings.ImgurSignedIn = value; base.RaisePropertyChanged(); }
        }

        public string ImgurUsername
        {
            get { return _settings.ImgurUsername; }
            set { _settings.ImgurUsername = value; base.RaisePropertyChanged(); }
        }

        public void LogoutOfImgur()
        {
            try
            {
                var manager = new ImgurManager();
                manager.RemoveTokens(_settings.ImgurUsername);
                ImgurUsername = string.Empty;
                ImgurSignedIn = false;
            }
            catch (Exception)
            {

            }
        }

        public async void LoginToImgur()
        {
            try
            {
                var manager = new ImgurManager();
                var username = await manager.ImgurLogin();
                if (string.IsNullOrEmpty(username)) return;
                ImgurSignedIn = true;
                ImgurUsername = username;
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
        }
    }

    public class AboutPartViewModel : MazuiViewModel
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }

        public Uri RateMe => new Uri("https://somethingawful.com");
    }
}
