using Mazui.Database.Context;
using Mazui.Services;
using Mazui.Tools.BackgroundTasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Mazui.Tools.ViewSystem;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Background;

namespace Mazui
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        SettingsService _settingsService = SettingsService.Instance;

        public ObservableCollection<ViewLifetimeControl> SecondaryViews = new ObservableCollection<ViewLifetimeControl>();

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
            #region Xbox
            if (IsTenFoot)
            {
                this.RequiresPointerMode = Windows.UI.Xaml.ApplicationRequiresPointerMode.WhenRequested;
            }
            #endregion
            #region Database
            using (var db = new UserAuthContext())
            {
                db.Database.Migrate();
            }
            using (var db = new ForumsContext())
            {
                db.Database.Migrate();
            }
            #endregion

            RequestedTheme = _settingsService.AppTheme;
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            return new ModalDialog()
            {
                ModalContent = new Views.Busy(),
                Content = new Views.Shell(service)
            };
        }

        public void SetTitleBarColor()
        {
            //windows title bar      
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = (Color)Application.Current.Resources["SystemAccentColor"];
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.ForegroundColor = Colors.White;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemAccentColor"];
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Colors.White;

            //StatusBar for Mobile

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = (Color)Application.Current.Resources["SystemAccentColor"];
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
            }
        }

        public override void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
            SetTitleBarColor();
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(330, 200));
            SetupBackgroundServices();
            SetTitleBarColor();
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }

        private async void SetupBackgroundServices()
        {
			IsIoT = ApiInformation.IsTypePresent("Windows.Devices.Gpio.GpioController");

			if (IsIoT) return;

			TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
			BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.ToastBackgroundTaskName);
			var task2 = await
				BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.ToastBackgroundTaskEntryPoint,
					BackgroundTaskUtils.ToastBackgroundTaskName, new ToastNotificationActionTrigger(),
					null);

			if (SettingsService.Instance.BackgroundEnable)
			{
				BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
				var task = await
					BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
						BackgroundTaskUtils.BackgroundTaskName,
						new TimeTrigger(15, false),
						null);
			}
		}

		#region iot
		public static bool IsIoT { get; private set; } = false;
		#endregion

		#region Xbox
		public static bool IsTenFootPC { get; private set; } = false;

        public static bool IsTenFoot
        {
            get
            {
                return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox" || IsTenFootPC;
            }
            set
            {
                if (value != IsTenFootPC)
                {
                    IsTenFootPC = value;
                    TenFootModeChanged?.Invoke(null, null);
                }
            }
        }

        public static event EventHandler TenFootModeChanged;

        #endregion
    }
}
