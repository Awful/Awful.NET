using Mazui.Core.Managers;
using Mazui.Core.Models.Threads;
using Mazui.Core.Models.Users;
using Mazui.Database.Functions;
using Mazui.Services;
using Mazui.Tools.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;

namespace Mazui.Tools.BackgroundTasks
{
    public class BackgroundActivity
    {
        BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        volatile bool _cancelRequested = false;
        BackgroundTaskDeferral _deferral = null;
        ThreadPoolTimer _periodicTimer = null;
        uint _progress = 0;
        IBackgroundTaskInstance _taskInstance = null;
        WebManager _webManager;
        ThreadManager _threadManager;
        UserAuth _user = default(UserAuth);
        bool _isLoggedIn;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Setup the background task.
            Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
            _deferral = taskInstance.GetDeferral();
            _taskInstance = taskInstance;
            await LoginUser();

            if (_isLoggedIn)
            {
                _threadManager = new ThreadManager(_webManager);
                var bookmarks = await GetNewBookmarks();
                switch (taskInstance.Task.Name)
                {
                    case "BookmarkNotifyBackgroundActivity":
                        await BookmarkNotifyBackgroundActivity(bookmarks);
                        break;
                    case "BookmarkBackgroundActivity":
                        await BookmarkBackgroundActivity(bookmarks);
                        break;
                }
            }
            _deferral.Complete();
        }

        public async Task BookmarkNotifyBackgroundActivity(List<Thread> bookmarks)
        {
            
        }

        public async Task BookmarkBackgroundActivity(List<Thread> bookmarks)
        {

        }

        private  async Task<List<Thread>> GetNewBookmarks()
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
                SettingsService.Instance.LastRefresh = DateTime.UtcNow;
                await ForumsDatabase.RefreshBookmarkedThreads(newbookmarkthreads.ToList());
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                // Failed to get bookmarks, return empty.
            }
            return newbookmarkthreads;
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;
            _cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }

        private async Task LoginUser()
        {
            var auth = await UserHandler.GetDefaultAuthWebManager();
            _webManager = auth.WebManager;
            _isLoggedIn = auth.IsLoggedIn;
            _user = auth.User;
        }

        public static void Start(IBackgroundTaskInstance taskInstance)
        {
            var activity = new BackgroundActivity();
            activity.Run(taskInstance);
        }
    }
}
