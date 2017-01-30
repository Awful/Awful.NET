using Mazui.Core.Managers;
using Mazui.Core.Models.Threads;
using Mazui.Database.Functions;
using Mazui.Tools;
using Mazui.Tools.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Mazui.ViewModels
{
    public class ForumThreadListBaseViewModel : MazuiViewModel
    {
        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public async Task NavigateToThread(Thread thread)
        {
            Selected = thread;
            await NavigationService.NavigateAsync(typeof(Views.ThreadPage), JsonConvert.SerializeObject(thread));
        }

        public async void GoToLastPage(Thread thread)
        {
            thread.CurrentPage = thread.TotalPages;
            thread.RepliesSinceLastOpened = 0;
            Selected = thread;
        }

        public async void UnreadThread(Thread thread)
        {
            var threadManager = new ThreadManager(WebManager);
            await threadManager.MarkThreadUnreadAsync(thread.ThreadId);
            thread.HasBeenViewed = false;
            thread.HasSeen = false;
            thread.ReplyCount = 0;
            var msgDlg2 =
                       new MessageDialog(string.Format("'{0}' is now unread.", thread.Name))
                       {
                           DefaultCommandIndex = 1
                       };
            await msgDlg2.ShowAsync();
        }

        public async void AddRemoveBookmark(Thread thread)
        {
            string error = "";
            try
            {
                var threadManager = new ThreadManager(WebManager);
                string bookmarkstring;
                if (thread.IsBookmark)
                {
                    await threadManager.RemoveBookmarkAsync(thread.ThreadId);
                    thread.IsBookmark = !thread.IsBookmark;
                    await ForumsDatabase.RefreshBookmark(thread);
                    bookmarkstring = string.Format("'{0}' has been removed from your bookmarks.", thread.Name);
                }
                else
                {
                    bookmarkstring = string.Format("'{0}' has been added to your bookmarks.",
                        thread.Name);
                    thread.IsBookmark = !thread.IsBookmark;
                    await threadManager.AddBookmarkAsync(thread.ThreadId);
                    await ForumsDatabase.AddBookmark(thread);
                }
                var msgDlg2 =
                       new MessageDialog(bookmarkstring)
                       {
                           DefaultCommandIndex = 1
                       };
                await msgDlg2.ShowAsync();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (!string.IsNullOrEmpty(error)) await ResultChecker.SendMessageDialogAsync($"Failed to get Bookmarks: {error}", false);
        }
    }
}
