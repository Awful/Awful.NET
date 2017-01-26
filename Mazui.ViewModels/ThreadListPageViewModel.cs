using Mazui.Core.Models.Forums;
using Mazui.Core.Models.Threads;
using Mazui.Core.Tools;
using Mazui.Tools;
using Mazui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Mazui.ViewModels
{
    public class ThreadListPageViewModel : ForumThreadListBaseViewModel
    {
        #region Properties

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }


        private PageScrollingCollection _ForumPageScrollingCollection = default(PageScrollingCollection);
        public PageScrollingCollection ForumPageScrollingCollection
        {
            get { return _ForumPageScrollingCollection; }
            set
            {
                Set(ref _ForumPageScrollingCollection, value);
            }
        }

        private Forum _forum = default(Forum);
        public Forum Forum
        {
            get { return _forum; }
            set
            {
                Set(ref _forum, value);
            }
        }

        #endregion
        #region Methods
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (WebManager == null)
            {
                await LoginUser();
            }

            SuspendRecover(parameter, suspensionState);

            // If we're recovering from suspension, but the view model has not been cleared.
            // Keep the state we currently have. Everything is already populated.
            if (Forum != null && (mode == NavigationMode.Forward | mode == NavigationMode.Back)) return;


            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1, WebManager);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[EndPoints.SavedForum] = JsonConvert.SerializeObject(Forum);
            }

            return Task.CompletedTask;
        }

        public async Task NavigateToThread(Thread thread)
        {
            Selected = thread;
            await NavigationService.NavigateAsync(typeof(Views.ThreadPage), JsonConvert.SerializeObject(thread));
        }

        private void SuspendRecover(object parameter, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(EndPoints.SavedForum))
            {
                if (Forum == null)
                {
                    Forum = JsonConvert.DeserializeObject<Forum>(suspensionState[EndPoints.SavedForum]?.ToString());
                }
            }
            else
            {
                var forum = JsonConvert.DeserializeObject<Forum>((string)parameter);
                if (forum == null) return;
                Forum = forum;
            }

            if (suspensionState.ContainsKey(EndPoints.SavedThread))
            {
                var thread = JsonConvert.DeserializeObject<Thread>(suspensionState[EndPoints.SavedThread]?.ToString());
                if (Selected != null && Selected.ThreadId == thread.ThreadId)
                {
                    Selected.RepliesSinceLastOpened = thread.RepliesSinceLastOpened;
                }
            }
        }

        private async void ForumPageScrollingCollection_CheckIsPaywallEvent(object sender, PageScrollingCollection.IsPaywallArgs e)
        {
            if (!e.IsPaywall) return;
            await NavigationService.NavigateAsync(typeof(PaywallPage));
        }
        #endregion
    }
}
