using Awful.Database.Functions;
using Awful.Helpers;
using Awful.Parser.Managers;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Users;
using Awful.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Awful.Database.Context;
using Awful.Parser.Core;
using Awful.Tools;

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

        public WebClient WebManager { get; set; }

        public async Task AddRemoveBookmark(Thread thread)
        {
            string error = "";
            try
            {
                var threadManager = new BookmarkManager(WebManager);
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
        }

        public async Task AddRemoveNotification(Thread thread)
        {
            string error = "";
            try
            {
                if (!thread.IsBookmark) return;
                thread.IsNotified = !thread.IsNotified;
                await ForumsDatabase.AddRemoveNotification(thread.ThreadId, thread.IsNotified);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
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
