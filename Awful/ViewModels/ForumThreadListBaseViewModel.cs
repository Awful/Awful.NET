using Awful.Controls;
using Awful.Parser.Managers;
using Awful.Parser.Models.Threads;
using Awful.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Awful.ViewModels
{
    public class ForumThreadListBaseViewModel : AwfulViewModel
    {
        public ThreadView ThreadView { get; set; }

        public MasterDetailViewControl MasterDetailViewControl { get; set; }

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private bool _isThreadSelectedAndLoaded;

        public bool IsThreadSelectedAndLoaded
        {
            get { return _isThreadSelectedAndLoaded; }
            set
            {
                Set(ref _isThreadSelectedAndLoaded, value);
            }
        }

        public async Task NavigateToThread(Thread thread)
        {
            Selected = thread;
            //await NavigationService.NavigateAsync(typeof(Views.ThreadPage), JsonConvert.SerializeObject(thread));
        }

        public void GoToLastPage(Thread thread)
        {
            thread.CurrentPage = thread.TotalPages;
            thread.RepliesSinceLastOpened = 0;
            Selected = thread;
        }

        public async Task UnreadThread(Thread thread)
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
    }
}
