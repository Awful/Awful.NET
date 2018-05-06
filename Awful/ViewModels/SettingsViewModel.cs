using System;
using System.Windows.Input;

using Awful.Helpers;
using Awful.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Awful.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : Observable
    {
        readonly SettingsService _settings;

        #region Settings

        public bool InfinitePageScrolling
        {
            get { return _settings.InfinitePageScrolling; }
            set { _settings.InfinitePageScrolling = value; OnPropertyChanged("InfinitePageScrolling"); }
        }

        public bool UseShowEmbeddedGifvButton
        {
            get { return _settings.ShowEmbeddedGifv; }
            set { _settings.ShowEmbeddedGifv = value; OnPropertyChanged("UseShowEmbeddedGifvButton"); }
        }

        public bool UseShowEmbeddedTweetsButton
        {
            get { return _settings.ShowEmbeddedTweets; }
            set { _settings.ShowEmbeddedTweets = value; OnPropertyChanged("UseShowEmbeddedTweetsButton"); }
        }

        public bool UseShowEmbeddedVideoButton
        {
            get { return _settings.ShowEmbeddedVideo; }
            set { _settings.ShowEmbeddedVideo = value; OnPropertyChanged("UseShowEmbeddedVideoButton"); }
        }

        public bool AlwaysAutoplayGif
        {
            get { return _settings.AutoplayGif; }
            set { _settings.AutoplayGif = value; OnPropertyChanged("AlwaysAutoplayGif"); }
        }

        public bool UseBackgroundTask
        {
            get { return _settings.BackgroundEnable; }
            set { _settings.BackgroundEnable = value; OnPropertyChanged("UseBackgroundTask"); }
        }

        public bool UseBackgroundBookmarkLiveTile
        {
            get { return _settings.BookmarkBackground; }
            set { _settings.BookmarkBackground = value; OnPropertyChanged("UseBackgroundBookmarkLiveTile"); }
        }

        public bool UseTransparentThreadListBackground
        {
            get { return _settings.TransparentThreadListBackground; }
            set { _settings.TransparentThreadListBackground = value; OnPropertyChanged("UseTransparentThreadListBackground"); }
        }

        public bool UseBookmarkBackgroundNotify
        {
            get { return _settings.BookmarkNotifications; }
            set { _settings.BookmarkNotifications = value; OnPropertyChanged("UseBookmarkBackgroundNotify"); }
        }

        public bool ImgurSignedIn
        {
            get { return _settings.ImgurSignedIn; }
            set { _settings.ImgurSignedIn = value; OnPropertyChanged("ImgurSignedIn"); }
        }

        public string ImgurUsername
        {
            get { return _settings.ImgurUsername; }
            set { _settings.ImgurUsername = value; OnPropertyChanged("ImgurUsername"); }
        }


        #endregion

        #region Theme

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        #endregion

        public SettingsViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = SettingsService.Instance;
        }

        public void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public void LogoutOfImgur()
        {
            try
            {
                //var manager = new ImgurManager();
                //manager.RemoveTokens(_settings.ImgurUsername);
                //ImgurUsername = string.Empty;
                //ImgurSignedIn = false;
            }
            catch (Exception)
            {

            }
        }

        public async void LoginToImgur()
        {
            try
            {
                //var manager = new ImgurManager();
                //var username = await manager.ImgurLogin();
                //if (string.IsNullOrEmpty(username)) return;
                //ImgurSignedIn = true;
                //ImgurUsername = username;
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
        }

    }
}
