using Mazui.Core.Managers;
using Mazui.Core.Models.Threads;
using Mazui.Core.Models.Users;
using Mazui.Database.Functions;
using Mazui.Notifications;
using Mazui.Tools.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Mazui.BackgroundNotify
{
	public sealed class BackgroundNotifyStatus : IBackgroundTask
	{
		WebManager _webManager;
		ThreadManager _threadManager;
		UserAuth _user = default(UserAuth);
		bool _isLoggedIn;
		readonly Template10.Services.SettingsService.ISettingsHelper _helper;

		public BackgroundNotifyStatus()
		{
			_helper = new Template10.Services.SettingsService.SettingsHelper();
		}

		public async void Run(IBackgroundTaskInstance taskInstance)
		{
			BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

			try
            {
                if (_helper.Read<bool>("BackgroundEnable", false))
                {
					await LoginUser();
					if (_isLoggedIn)
					{
						_threadManager = new ThreadManager(_webManager);
						var bookmarks = await GetNewBookmarks();
						if (_helper.Read<bool>("BookmarkBackground", false))
						{
							BookmarkBackgroundActivity(bookmarks);

						}
						if (_helper.Read<bool>("BookmarkNotifications", false))
						{
							BookmarkNotifyBackgroundActivity(bookmarks);
						}
					}
				}

            }
            catch (Exception)
            {
            }

			deferral.Complete();
		}

		private void BookmarkNotifyBackgroundActivity(IList<Thread> bookmarks)
		{
			foreach (var thread in bookmarks.Where(thread => thread.RepliesSinceLastOpened > 0 && thread.IsNotified))
			{
				NotifyStatusTile.CreateToastNotification(thread);
			}
		}

		private void BookmarkBackgroundActivity(IList<Thread> bookmarks)
		{
			foreach (var thread in bookmarks.Where(thread => thread.RepliesSinceLastOpened > 0))
			{
				NotifyStatusTile.CreateBookmarkLiveTile(thread);
			}
		}

		private async Task<IList<Thread>> GetNewBookmarks()
		{
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
				_helper.Write<DateTime>("LastRefresh", DateTime.UtcNow);
				await ForumsDatabase.RefreshBookmarkedThreads(newbookmarkthreads.ToList());
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				// Failed to get bookmarks, return empty.
			}
			return await ForumsDatabase.GetBookmarkedThreadsFromDb();
		}

		private async Task LoginUser()
		{
			var auth = await UserHandler.GetDefaultAuthWebManager();
			_webManager = auth.WebManager;
			_isLoggedIn = auth.IsLoggedIn;
			_user = auth.User;
		}
	}
}
