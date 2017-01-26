using Mazui.Core.Models.Forums;
using Mazui.Core.Models.Threads;
using Mazui.Tools;
using Mazui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Mazui.ViewModels
{
    public class ThreadListPageViewModel : ForumThreadListBaseViewModel
    {
        #region Properties

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

            // If we're recovering from suspension, but the view model has not been cleared.
            // Keep the state we currently have. Everything is already populated.
            if (Forum != null && (mode == NavigationMode.Forward | mode == NavigationMode.Back)) return;

            SuspendRecover(parameter, suspensionState);

            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1, WebManager);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;
        }

        public async Task NavigateToThread(Thread thread)
        {
            await NavigationService.NavigateAsync(typeof(Views.ThreadPage), JsonConvert.SerializeObject(thread));
        }

        private void SuspendRecover(object parameter, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(Forum)))
            {
                if (Forum == null)
                {
                    Forum = JsonConvert.DeserializeObject<Forum>(suspensionState[nameof(Forum)]?.ToString());
                }
            }
            else
            {
                var forum = JsonConvert.DeserializeObject<Forum>((string)parameter);
                if (forum == null) return;
                Forum = forum;
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
