using Mazui.Core.Managers;
using Mazui.Core.Models.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Mazui.Tools
{
    public class PrivateMessageScrollingCollection : ObservableCollection<PrivateMessage>, ISupportIncrementalLoading
    {
        public PrivateMessageScrollingCollection(WebManager web)
        {
            HasMoreItems = true;
            IsLoading = false;
            PageCount = 0;
            privateMessageManager = new PrivateMessageManager(web);
        }

        private PrivateMessageManager privateMessageManager;
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

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            List<PrivateMessage> privateMessageList = new List<PrivateMessage>();
            try
            {
                var result = await privateMessageManager.GetPrivateMessagesAsync(PageCount);
                if (result.IsSuccess)
                {
                    privateMessageList = JsonConvert.DeserializeObject<List<PrivateMessage>>(result.ResultJson);
                }
            }
            catch (Exception ex)
            {
                // TODO: Find a better way to go into PM pages.
                privateMessageList = null;
                Debug.WriteLine("No more PMs...");
            }
            if (privateMessageList == null)
            {
                IsLoading = false;
                HasMoreItems = false;
                return new LoadMoreItemsResult { Count = count };
            }
            foreach (var item in privateMessageList)
            {
                Add(item);
            }
            if (privateMessageList.Any())
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

        public bool HasMoreItems { get; set; }
    }
}
