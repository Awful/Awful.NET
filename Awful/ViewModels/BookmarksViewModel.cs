using Awful.Database.Functions;
using Awful.Helpers;
using Awful.Managers;
using Awful.Models.Threads;
using Awful.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Awful.ViewModels
{
    public class BookmarksViewModel : ForumThreadListBaseViewModel
    {
        #region Properties

        private const string RefreshKey = "RefreshForumKey";

        private ObservableCollection<Thread> _bookmarkedThreads = new ObservableCollection<Thread>();


        public ObservableCollection<Thread> BookmarkedThreads
        {
            get { return _bookmarkedThreads; }
            set
            {
                Set(ref _bookmarkedThreads, value);
            }
        }

        private ThreadManager _threadManager;
        #endregion

        public async Task Load()
        {
            await Init();
        }

        public async Task LoadInitialList()
        {
            IsLoading = true;
            string error = "";
            try
            {
                var bookmarks = await ForumsDatabase.GetBookmarkedThreadsFromDb();
                if (bookmarks != null && bookmarks.Any())
                {
                    BookmarkedThreads = bookmarks.OrderBy(node => node.OrderNumber).ToObservableCollection();
                }
                string refreshTime = await ApplicationData.Current.LocalSettings.ReadAsync<string>(RefreshKey);
                if ((!BookmarkedThreads.Any() || (!string.IsNullOrEmpty(refreshTime) && DateTime.Parse(refreshTime) > (DateTime.UtcNow.AddHours(1.00)))))
                {
                    await Refresh();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (!string.IsNullOrEmpty(error)) await ResultChecker.SendMessageDialogAsync($"Failed to get Bookmarks (Load Inital List): {error}", false);
            IsLoading = false;
        }

        public async Task Init()
        {
            LoginUser();

            _threadManager = new ThreadManager(WebManager);

            if (BookmarkedThreads != null && BookmarkedThreads.Any())
            {
                return;
            }

            await LoadInitialList();
        }

        public async Task Refresh()
        {
            IsLoading = true;
            string error = "";
            try
            {
                var pageNumber = 1;
                var hasItems = false;
                var oldList = true;
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

                    if (oldList)
                    {
                        BookmarkedThreads = new ObservableCollection<Thread>();
                        oldList = false;
                    }

                    foreach (var bookmark in bookmarks)
                    {
                        BookmarkedThreads.Add(bookmark);
                    }
                }
                await ApplicationData.Current.LocalSettings.SaveAsync(RefreshKey, DateTime.UtcNow);
                await ForumsDatabase.RefreshBookmarkedThreads(BookmarkedThreads.ToList());
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (!string.IsNullOrEmpty(error)) await ResultChecker.SendMessageDialogAsync($"Failed to get Bookmarks (Refresh): {error}", false);
            IsLoading = false;
        }
    }
}
