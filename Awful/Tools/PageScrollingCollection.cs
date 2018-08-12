using Awful.Parser.Core;
using Awful.Parser.Managers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Awful.Tools
{
    public class PageScrollingCollection : ObservableCollection<Thread>, ISupportIncrementalLoading
    {
        public PageScrollingCollection(Forum forumEntity, int pageCount, WebClient web)
        {
            HasMoreItems = true;
            IsLoading = false;
            PageCount = pageCount;
            ForumEntity = forumEntity;
            _threadManager = new ThreadListManager(web);
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public delegate void CheckIsPaywall(object sender, IsPaywallArgs e);
        public class IsPaywallArgs : EventArgs
        {
            public bool IsPaywall;
        }
        public event CheckIsPaywall CheckIsPaywallEvent;
        private void FireEvent(bool isPaywall)
        {
            IsPaywallArgs e1 = new IsPaywallArgs { IsPaywall = isPaywall };
            CheckIsPaywallEvent?.Invoke(this, e1);
            e1 = null;
        }

        private readonly ThreadListManager _threadManager;

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            List<Thread> forumThreadEntities;
            try
            {
                forumThreadEntities = await _threadManager.GetForumThreadListAsync(ForumEntity, PageCount);
            }
            catch (Exception ex)
            {
                if (ex.Message == "paywall")
                {
                    FireEvent(true);
                }
                HasMoreItems = false;
                IsLoading = false;
                return new LoadMoreItemsResult { Count = count };
            }

            foreach (var forumThreadEntity in forumThreadEntities.Where(forumThreadEntity => !forumThreadEntity.IsAnnouncement))
            {
                Add(forumThreadEntity);
            }
            if (forumThreadEntities.Any(node => !node.IsAnnouncement))
            {
                HasMoreItems = true;
                PageCount++;
            }
            else
            {
                HasMoreItems = false;
            }
            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }


        private Forum ForumEntity { get; set; }

        private int PageCount { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
            }
        }

        public bool HasMoreItems { get; set; }
    }
}
