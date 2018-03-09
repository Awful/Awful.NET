using Awful.Managers;
using Awful.Models.Forums;
using Awful.Models.Threads;
using Awful.Models.Web;
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
        public PageScrollingCollection(Forum forumEntity, int pageCount, WebManager web)
        {
            HasMoreItems = true;
            IsLoading = false;
            PageCount = pageCount;
            ForumEntity = forumEntity;
            _threadManager = new ThreadManager(web);
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

        private readonly ThreadManager _threadManager;

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            List<Thread> forumThreadEntities;
            try
            {
                var result = await _threadManager.GetForumThreadsAsync(ForumEntity.Location, ForumEntity.ForumId, PageCount);
                var resultCheck = await ResultChecker.CheckPaywallOrSuccess(result);
                if (!resultCheck)
                {
                    if (result.Type == typeof(Error).ToString())
                    {
                        var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
                        if (error.IsPaywall)
                        {
                            FireEvent(true);
                        }
                    }
                    HasMoreItems = false;
                    IsLoading = false;
                    return new LoadMoreItemsResult { Count = count };
                }
                forumThreadEntities = JsonConvert.DeserializeObject<List<Thread>>(result.ResultJson);
            }
            catch (Exception ex)
            {
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
