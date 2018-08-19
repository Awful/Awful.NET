using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.Tools;
using Awful.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Awful.ViewModels
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

        public void Load(string forumJson)
        {

            Forum = JsonConvert.DeserializeObject<Forum>(forumJson);
            MasterDetailViewControl.SetMasterHeaderText(Forum.Name);
            Init();
        }

        public void Init()
        {
            LoginUser();

            if (ForumPageScrollingCollection != null) ForumPageScrollingCollection.CheckIsPaywallEvent -= ForumPageScrollingCollection_CheckIsPaywallEvent;
            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1, WebManager);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;
        }
        private async void ForumPageScrollingCollection_CheckIsPaywallEvent(object sender, PageScrollingCollection.IsPaywallArgs e)
        {
            if (!e.IsPaywall) return;
            NavigationService.Navigate(typeof(PaywallPage));
        }

        public void CreateThread()
        {
            if (this.IsLoggedIn)
            {
                NavigationService.Navigate(typeof(NewThreadPage), JsonConvert.SerializeObject(Forum));
            }
        }

        #endregion
    }
}
