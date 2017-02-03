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
using Mazui.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Mazui.Tools.ViewSystem;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.ViewManagement;
using Template10.Common;
using Mazui.Views;

namespace Mazui.ViewModels
{
    public class ForumThreadListBaseViewModel : MazuiViewModel
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

        public async Task OpenInNewWindow(Thread thread)
        {
            var control = await NavigationService.OpenAsync(typeof(ThreadPage), JsonConvert.SerializeObject(thread));
        }

        public async Task NavigateToThread(Thread thread)
        {
            Selected = thread;
            await NavigationService.NavigateAsync(typeof(Views.ThreadPage), JsonConvert.SerializeObject(thread));
        }

        public async Task GoToLastPage(Thread thread)
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
