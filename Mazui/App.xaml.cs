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
using Newtonsoft.Json;
using Mazui.Notifications;
using Windows.Storage;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;

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

            RequestedTheme = IsTenFoot ? ApplicationTheme.Dark : _settingsService.AppTheme;

            MobileCenter.Start("3d56682d-2a46-4cdc-91d4-3b605577d728", typeof(Analytics));
        }

		public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            if (!IsTenFoot)
			{
				var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
				return new ModalDialog()
				{
				    DisableBackButtonWhenModal = true,
                    ModalContent = new Views.Busy(),
					Content = new Views.Shell(service)
				};
			}
			else
			{
				var navigationFrame = new Frame();
				var navigationService = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, navigationFrame);
				return new ModalDialog
				{
					DisableBackButtonWhenModal = true,
					Content = navigationFrame
				};
			}
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

			if (IsTenFoot)
			{
				Application.Current.Resources["SystemControlHighlightAccentBrush"] = Colors.Black;
			}
        }

        public override void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
            SetTitleBarColor();
        }

		public override async Task OnInitializeAsync(IActivatedEventArgs args)
		{
			await InstallVoiceCommands();
		}

		public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(330, 200));
			if (IsTenFoot)
			{
				// Turn off overscan. We'll be handling it.
				var AppView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
				AppView.SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
			}
			SetupBackgroundServices();
            SetTitleBarColor();
			await SetupStartupLocation(startKind, args);
        }

		private async Task SetupStartupLocation(StartKind startKind, IActivatedEventArgs args)
		{
			if (IsTenFoot)
			{
				await NavigationService.NavigateAsync(typeof(XboxViews.MainPage));
				return;
			}
			try
			{
				if (startKind == StartKind.Activate)
				{
					if (args.Kind == ActivationKind.ToastNotification)
						StartupFromToast(args);
					if (args.Kind == ActivationKind.VoiceCommand)
						await StartupFromVoice(args);
					if (args.Kind == ActivationKind.Protocol)
						StartupFromProtocol(args);
				}
				else
				{
					await NavigationService.NavigateAsync(typeof(Views.MainPage));
				}
			}
			catch (Exception)
			{
				// If all else fails, go to the main page.
				await NavigationService.NavigateAsync(typeof(Views.MainPage));
			}
		}

		private async Task InstallVoiceCommands()
		{
			try
			{
				// Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
				// version, it's not unreasonable to do this upon app load.
				StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"MazuiCommands.xml");

				await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
			}
		}

		private async void StartupFromToast(IActivatedEventArgs args)
		{
			var toastArgs = args as ToastNotificationActivatedEventArgs;
			if (toastArgs == null)
				return;
			var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(toastArgs.Argument);
			await NavigationService.NavigateAsync(typeof(Views.BookmarkPage), arguments);
		}

		private async Task StartupFromVoice(IActivatedEventArgs args)
		{
			var commandArgs = args as VoiceCommandActivatedEventArgs;
			await HandleVoiceRequest(commandArgs);
		}

		private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
		{
			return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
		}

		private async Task HandleVoiceRequest(VoiceCommandActivatedEventArgs commandArgs)
		{
			SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

			// Get the name of the voice command and the text spoken. See AdventureWorksCommands.xml for
			// the <Command> tags this can be filled with.
			string voiceCommandName = speechRecognitionResult.RulePath[0];
			string textSpoken = speechRecognitionResult.Text;

			// The commandMode is either "voice" or "text", and it indictes how the voice command
			// was entered by the user.
			// Apps should respect "text" mode by providing feedback in silent form.
			string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);



			switch (voiceCommandName)
			{
				case "openBookmarks":
					await NavigationService.NavigateAsync(typeof(Views.BookmarkPage));
					break;
				case "openPrivateMessages":
					await NavigationService.NavigateAsync(typeof(Views.PrivateMessageListPage));
					break;
				case "lowtaxIsAJerk":
					// TODO: Maybe fix this? Not like anyone would care.
					await NavigationService.NavigateAsync(typeof(Views.BookmarkPage));
					break;
				default:
					await NavigationService.NavigateAsync(typeof(Views.MainPage));
					break;
			}
		}

		private async void StartupFromProtocol(IActivatedEventArgs args)
		{
			var protoArgs = args as ProtocolActivatedEventArgs;
			var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(protoArgs.Uri.OriginalString.Replace("awful:", ""));
			if (arguments != null && arguments.ThreadId > 0 && arguments.IsThreadBookmark)
			{
				await NavigationService.NavigateAsync(typeof(Views.BookmarkPage), arguments);
			}
			else if (arguments != null && arguments.ThreadId > 0 && !arguments.IsThreadBookmark)
			{
				await NavigationService.NavigateAsync(typeof(Views.ThreadPage), arguments);
			}
			else
			{
				await NavigationService.NavigateAsync(typeof(Views.MainPage));
			}
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
                return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox" || Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Holographic" || IsTenFootPC;
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
