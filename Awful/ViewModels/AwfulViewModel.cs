using Awful.Database.Functions;
using Awful.Helpers;
using Awful.Managers;
using Awful.Models.Threads;
using Awful.Models.Users;
using Awful.Parsers;
using Awful.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Awful.ViewModels
{
    public class AwfulViewModel : Observable
    {
        private bool _isLoggedIn = default(bool);

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
            }
        }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private UserAuth _user = default(UserAuth);

        public UserAuth User
        {
            get { return _user; }
            set
            {
                Set(ref _user, value);
            }
        }

        public WebManager WebManager { get; set; }

        public async Task AddRemoveBookmark(Thread thread)
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
                    bookmarkstring = string.Format("'{0}' has been removed from your bookmarks.", thread.Name);
                }
                else
                {
                    bookmarkstring = string.Format("'{0}' has been added to your bookmarks.",
                        thread.Name);
                    thread.IsBookmark = !thread.IsBookmark;
                    await threadManager.AddBookmarkAsync(thread.ThreadId);
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

        public async Task AddRemoveNotification(Thread thread)
        {
            string error = "";
            try
            {
                if (!thread.IsBookmark) return;
                thread.IsNotified = !thread.IsNotified;
                await ForumsDatabase.AddRemoveNotification(thread.ThreadId, thread.IsNotified);
                await ResultChecker.SendMessageDialogAsync(thread.IsNotified ? $"You will now be notified of updates to '{thread.Name}'." : $"'{thread.Name}' is now removed for your notification list.", true);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (!string.IsNullOrEmpty(error)) await ResultChecker.SendMessageDialogAsync($"Failed to get Bookmarks: {error}", false);
        }

        public void TestLoginUser()
        {
            var auth = UserAuthHandler.GetDefaultAuthWebManager();
            User = auth.User;
            IsLoggedIn = auth.IsLoggedIn;
        }

        public void LoginUser()
        {
            var auth = UserAuthHandler.GetDefaultAuthWebManager();
            User = auth.User;
            IsLoggedIn = auth.IsLoggedIn;
            WebManager = auth.WebManager;
        }
    }
}
