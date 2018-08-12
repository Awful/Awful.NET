using Awful.Database.Functions;
using Awful.Helpers;
using Awful.Parser.Managers;
using Awful.Parser.Models.Threads;
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

        private BookmarkManager _threadManager;
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

            _threadManager = new BookmarkManager(WebManager);

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
                var bookmarkResult = await _threadManager.GetAllBookmarks();
                BookmarkedThreads = new ObservableCollection<Thread>();
                foreach (var bookmark in bookmarkResult)
                {
                    BookmarkedThreads.Add(bookmark);
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
