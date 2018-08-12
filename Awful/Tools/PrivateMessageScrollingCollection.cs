using Awful.Parser.Core;
using Awful.Parser.Managers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Messages;
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
    public class PrivateMessageScrollingCollection : ObservableCollection<PrivateMessage>, ISupportIncrementalLoading
    {
        public PrivateMessageScrollingCollection(WebClient web)
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
                privateMessageList = await privateMessageManager.GetPrivateMessageListAsync(PageCount);
            }
            catch (Exception ex)
            {
                // TODO: Find a better way to go into PM pages.
                privateMessageList = null;
                Console.WriteLine("No more PMs...");
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
