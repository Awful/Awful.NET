using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Mazui.Core.Managers;
using Mazui.Database;
using Mazui.Notifications;
using Newtonsoft.Json;
using Mazui.Tools.Authentication;
using Mazui.Core.Models.Messages;
using Mazui.Core.Models.Threads;
using Mazui.Database.Functions;
using Mazui.Core.Models.Users;

namespace Mazui.VoiceCommands
{
	public sealed class MazuiVoiceCommandService : IBackgroundTask
	{
		readonly Template10.Services.SettingsService.ISettingsHelper _helper;

		public MazuiVoiceCommandService()
		{
			_helper = new Template10.Services.SettingsService.SettingsHelper();
		}

		/// <summary>
		/// the service connection is maintained for the lifetime of a cortana session, once a voice command
		/// has been triggered via Cortana.
		/// </summary>
		VoiceCommandServiceConnection voiceServiceConnection;

		/// <summary>
		/// Lifetime of the background service is controlled via the BackgroundTaskDeferral object, including
		/// registering for cancellation events, signalling end of execution, etc. Cortana may terminate the 
		/// background service task if it loses focus, or the background task takes too long to provide.
		/// 
		/// Background tasks can run for a maximum of 30 seconds.
		/// </summary>
		BackgroundTaskDeferral serviceDeferral;

		/// <summary>
		/// ResourceMap containing localized strings for display in Cortana.
		/// </summary>
		ResourceMap cortanaResourceMap;

		/// <summary>
		/// The context for localized strings.
		/// </summary>
		ResourceContext cortanaContext;

		/// <summary>
		/// Get globalization-aware date formats.
		/// </summary>
		DateTimeFormatInfo dateFormatInfo;

		bool _isLoggedIn;
		UserAuth _user = default(UserAuth);

		private async Task LoginUser()
		{
			var auth = await UserHandler.GetDefaultAuthWebManager();
			_webManager = auth.WebManager;
			_isLoggedIn = auth.IsLoggedIn;
			_user = auth.User;
			_threadManager = new ThreadManager(_webManager);
			_privateMessageManager = new PrivateMessageManager(_webManager);
		}

		public async void Run(IBackgroundTaskInstance taskInstance)
		{
			serviceDeferral = taskInstance.GetDeferral();

			// Register to receive an event if Cortana dismisses the background task. This will
			// occur if the task takes too long to respond, or if Cortana's UI is dismissed.
			// Any pending operations should be cancelled or waited on to clean up where possible.
			taskInstance.Canceled += OnTaskCanceled;

			var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

			// Load localized resources for strings sent to Cortana to be displayed to the user.
			cortanaResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

			// Select the system language, which is what Cortana should be running as.
			cortanaContext = ResourceContext.GetForViewIndependentUse();

			// Get the currently used system date format
			dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

			// This should match the uap:AppService and VoiceCommandService references from the 
			// package manifest and VCD files, respectively. Make sure we've been launched by
			// a Cortana Voice Command.
			if (triggerDetails != null && triggerDetails.Name == "MazuiVoiceCommandService")
			{
				try
				{
					voiceServiceConnection =
						VoiceCommandServiceConnection.FromAppServiceTriggerDetails(
							triggerDetails);

					voiceServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;

					VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
					await LoginUser();
					// Depending on the operation (defined in AdventureWorks:AdventureWorksCommands.xml)
					// perform the appropriate command.
					switch (voiceCommand.CommandName)
					{
						case "didMyThreadsUpdate":
							await CheckForBookmarksForUpdates();
							break;
						case "didMyPmUpdate":
							await CheckPmsForUpdates();
							break;
						default:
							// As with app activation VCDs, we need to handle the possibility that
							// an app update may remove a voice command that is still registered.
							// This can happen if the user hasn't run an app since an update.
							LaunchAppInForeground();
							break;
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("Handling Voice Command failed " + ex.ToString());
				}
			}
		}

		private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
		private WebManager _webManager;
		private ThreadManager _threadManager;
		private PrivateMessageManager _privateMessageManager;
		private async Task CheckPmsForUpdates()
		{
			string progressScreenString = "Checking for new private messages...";
			await ShowProgressScreen(progressScreenString);
			var privateMessagesJson = await _privateMessageManager.GetPrivateMessagesAsync(0);
			if (!privateMessagesJson.IsSuccess)
				return;
			var privateMessages = JsonConvert.DeserializeObject<List<PrivateMessage>>(privateMessagesJson.ResultJson);
			var userPrompt = new VoiceCommandUserMessage();

			VoiceCommandResponse response;

			if (!privateMessages.Any())
			{
				var userMessage = new VoiceCommandUserMessage();
				userMessage.DisplayMessage = userMessage.SpokenMessage = "You don't have any new private messages (because nobody likes you).";
				response = VoiceCommandResponse.CreateResponse(userMessage);
				await voiceServiceConnection.ReportSuccessAsync(response);
				return;
			}

			var newPms = privateMessages.Where(
				node =>
					!string.IsNullOrEmpty(node.Status) &&
					node.Status.Contains("newpm.gif"));
			if (!newPms.Any())
			{
				var userMessage = new VoiceCommandUserMessage();
				userMessage.DisplayMessage = userMessage.SpokenMessage = "You don't have any new private messages (because nobody likes you).";
				response = VoiceCommandResponse.CreateResponse(userMessage);
				await voiceServiceConnection.ReportSuccessAsync(response);
				return;
			}

			if (newPms.Count() == 1)
			{
				string newPmPrompt = string.Format("You have a new private message from {0}: \"{1}\", would you like to view it?",
					newPms.First().Sender, newPms.First().Title);
				userPrompt.DisplayMessage = userPrompt.SpokenMessage = newPmPrompt;
				var userReprompt = new VoiceCommandUserMessage();
				string newPmPromptConfirm = "Would you like to view it?";
				userReprompt.DisplayMessage = userReprompt.SpokenMessage = newPmPromptConfirm;

				response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

				var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);

				if (voiceCommandConfirmation != null)
				{
					if (voiceCommandConfirmation.Confirmed)
					{
						LaunchAppInForegroundPms();
					}
					else
					{
						// Confirm no action for the user.
						var userMessage = new VoiceCommandUserMessage();
						string dontShowAnythingMessage = string.Format("Well, {0} is going to be pretty mad you're blowing off their message. But I won't judge.", newPms.First().Sender);
						userMessage.DisplayMessage = userMessage.SpokenMessage = dontShowAnythingMessage;

						response = VoiceCommandResponse.CreateResponse(userMessage);
						await voiceServiceConnection.ReportSuccessAsync(response);
					}
				}
			}
			else
			{
				var newPmsString = string.Format("You have {0} new messages. Would you like to view them?", newPms.Count());
				userPrompt.DisplayMessage = userPrompt.SpokenMessage = newPmsString;
				var userReprompt = new VoiceCommandUserMessage();
				string newPmPromptConfirm = "Would you like to view them?";
				userReprompt.DisplayMessage = userReprompt.SpokenMessage = newPmPromptConfirm;

				response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

				var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);

				if (voiceCommandConfirmation != null)
				{
					if (voiceCommandConfirmation.Confirmed)
					{
						LaunchAppInForegroundPms();
					}
					else
					{
						// Confirm no action for the user.
						var userMessage = new VoiceCommandUserMessage();
						string dontShowAnythingMessage = "Well those people are going to be pretty mad at you ignoring them. But whatever, that's fine...";
						userMessage.DisplayMessage = userMessage.SpokenMessage = dontShowAnythingMessage;

						response = VoiceCommandResponse.CreateResponse(userMessage);
						await voiceServiceConnection.ReportSuccessAsync(response);
					}
				}

			}
		}

		private async void LaunchAppInForegroundPms()
		{
			var userMessage = new VoiceCommandUserMessage();
			userMessage.SpokenMessage = "Opening your private messages...";

			var response = VoiceCommandResponse.CreateResponse(userMessage);
			var toast = new ToastNotificationArgs() { Type = ToastType.Other, OpenPrivateMessages = true, IsLoggedIn = _isLoggedIn };
			response.AppLaunchArgument = JsonConvert.SerializeObject(toast);

			await voiceServiceConnection.RequestAppLaunchAsync(response);
		}
		private async Task CheckForBookmarksForUpdates()
		{
			// Begin loading data to search for the target store. If this operation is going to take a long time,
			// for instance, requiring a response from a remote web service, consider inserting a progress screen 
			// here, in order to prevent Cortana from timing out. 
			string progressScreenString = "Refreshing your bookmarks...";
			await ShowProgressScreen(progressScreenString);
			var newbookmarkthreads = new List<Thread>();
			try
			{
				var pageNumber = 1;
				var hasItems = false;
				while (!hasItems)
				{
					var bookmarkResult = await _threadManager.GetBookmarksAsync(pageNumber);
					var bookmarks = JsonConvert.DeserializeObject<List<Thread>>(bookmarkResult.ResultJson);
					if (!bookmarks.Any())
					{
						hasItems = true;
					}
					else
					{
						pageNumber++;
					}
					newbookmarkthreads.AddRange(bookmarks);
				}
				_helper.Read<DateTime>("LastRefresh", DateTime.UtcNow);
				await ForumsDatabase.RefreshBookmarkedThreads(newbookmarkthreads.ToList());
			}
			catch (Exception ex)
			{
				//AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
			}

			if (!newbookmarkthreads.Any())
			{
				return;
			}
			var threadsWithReplies = newbookmarkthreads.Where(node => node.RepliesSinceLastOpened > 0);

			var userPrompt = new VoiceCommandUserMessage();

			VoiceCommandResponse response;
			if (!threadsWithReplies.Any())
			{
				var userMessage = new VoiceCommandUserMessage();
				userMessage.DisplayMessage = userMessage.SpokenMessage = "Ehhh, I'm not seeing anything new here! Check again later.";
				response = VoiceCommandResponse.CreateResponse(userMessage);
				await voiceServiceConnection.ReportSuccessAsync(response);
			}
			else if (threadsWithReplies.Count() == 1)
			{
				// Prompt the user for confirmation that we've selected the correct trip to cancel.
				string threadBookmarkPrompt = string.Format("You have one thread with unread replies: {0}",
					threadsWithReplies.First().Name);
				userPrompt.DisplayMessage = userPrompt.SpokenMessage = threadBookmarkPrompt;
				var userReprompt = new VoiceCommandUserMessage();
				string threadBookmarkPromptConfirm = "Would you like to open up this thread?";
				userReprompt.DisplayMessage = userReprompt.SpokenMessage = threadBookmarkPromptConfirm;

				response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

				var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);

				// If RequestConfirmationAsync returns null, Cortana's UI has likely been dismissed.
				if (voiceCommandConfirmation != null)
				{
					if (voiceCommandConfirmation.Confirmed)
					{
						LaunchAppInForegroundWithThread(threadsWithReplies.First());
					}
					else
					{
						// Confirm no action for the user.
						var userMessage = new VoiceCommandUserMessage();
						string dontShowAnythingMessage = "Okay, I'll just keep doing the needful I guess.";
						userMessage.DisplayMessage = userMessage.SpokenMessage = dontShowAnythingMessage;

						response = VoiceCommandResponse.CreateResponse(userMessage);
						await voiceServiceConnection.ReportSuccessAsync(response);
					}
				}
			}
			else
			{
				string threadBookmarkPromptConfirm = "Would you like to open up this thread?";
				var recentThread = threadsWithReplies.First();
				var multipleThread = string.Format("You have {0} threads with unread replies.", threadsWithReplies.Count());

				var multipleReplies = string.Format("The most recent is \"{0}\" with {1} unread replies.", recentThread.Name, recentThread.RepliesSinceLastOpened);
				if (recentThread.RepliesSinceLastOpened == 1)
				{
					multipleReplies = string.Format("The most recent is \"{0}\" with {1} unread reply.", recentThread.Name, recentThread.RepliesSinceLastOpened); ;
				}
				string multipleThreads = string.Format("{0} {1} {2}", multipleThread, multipleReplies, threadBookmarkPromptConfirm);

				userPrompt.DisplayMessage = userPrompt.SpokenMessage = multipleThreads;
				var userReprompt = new VoiceCommandUserMessage();
				userReprompt.DisplayMessage = userReprompt.SpokenMessage = threadBookmarkPromptConfirm;

				response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

				var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);
				if (voiceCommandConfirmation != null)
				{
					if (voiceCommandConfirmation.Confirmed)
					{
						LaunchAppInForegroundWithThread(threadsWithReplies.First());
					}
					else
					{
						// Confirm no action for the user.
						var userMessage = new VoiceCommandUserMessage();
						string dontShowAnythingMessage = "Okay, I'll keep going back to whatever it was I was doing!";
						userMessage.DisplayMessage = userMessage.SpokenMessage = dontShowAnythingMessage;
						response = VoiceCommandResponse.CreateResponse(userMessage);
						await voiceServiceConnection.ReportSuccessAsync(response);
					}
				}
			}
		}

		/// <summary>
		/// Show a progress screen. These should be posted at least every 5 seconds for a 
		/// long-running operation, such as accessing network resources over a mobile 
		/// carrier network.
		/// </summary>
		/// <param name="message">The message to display, relating to the task being performed.</param>
		/// <returns></returns>
		private async Task ShowProgressScreen(string message)
		{
			var userProgressMessage = new VoiceCommandUserMessage();
			userProgressMessage.DisplayMessage = userProgressMessage.SpokenMessage = message;

			VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userProgressMessage);
			await voiceServiceConnection.ReportProgressAsync(response);
		}

		private async void LaunchAppInForegroundWithThread(Thread thread)
		{
			var userMessage = new VoiceCommandUserMessage();
			userMessage.SpokenMessage = "Opening the thread...";

			var response = VoiceCommandResponse.CreateResponse(userMessage);
			var notification = new ToastNotificationArgs() { Type = ToastType.Other, ThreadId = thread.ThreadId, IsLoggedIn = _isLoggedIn };
			response.AppLaunchArgument = JsonConvert.SerializeObject(notification);

			await voiceServiceConnection.RequestAppLaunchAsync(response);
		}

		/// <summary>
		/// Provide a simple response that launches the app. Expected to be used in the
		/// case where the voice command could not be recognized (eg, a VCD/code mismatch.)
		/// </summary>
		private async void LaunchAppInForeground()
		{
			var userMessage = new VoiceCommandUserMessage();
			userMessage.SpokenMessage = "Launching Awful...";

			var response = VoiceCommandResponse.CreateResponse(userMessage);

			response.AppLaunchArgument = "";

			await voiceServiceConnection.RequestAppLaunchAsync(response);
		}

		/// <summary>
		/// Handle the completion of the voice command. Your app may be cancelled
		/// for a variety of reasons, such as user cancellation or not providing 
		/// progress to Cortana in a timely fashion. Clean up any pending long-running
		/// operations (eg, network requests).
		/// </summary>
		/// <param name="sender">The voice connection associated with the command.</param>
		/// <param name="args">Contains an Enumeration indicating why the command was terminated.</param>
		private void OnVoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
		{
			this.serviceDeferral?.Complete();
		}

		/// <summary>
		/// When the background task is cancelled, clean up/cancel any ongoing long-running operations.
		/// This cancellation notice may not be due to Cortana directly. The voice command connection will
		/// typically already be destroyed by this point and should not be expected to be active.
		/// </summary>
		/// <param name="sender">This background task instance</param>
		/// <param name="reason">Contains an enumeration with the reason for task cancellation</param>
		private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			System.Diagnostics.Debug.WriteLine("Task cancelled, clean up");
			//Complete the service deferral
			this.serviceDeferral?.Complete();
		}
	}
}
