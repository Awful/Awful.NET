using Mazui.Core.Models.Messages;
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
    public class PrivateMessagesListViewModel : MazuiViewModel
    {
        private PrivateMessageScrollingCollection _privateMessageScrollingCollection = default(PrivateMessageScrollingCollection);
        public PrivateMessageScrollingCollection PrivateMessageScrollingCollection
        {
            get { return _privateMessageScrollingCollection; }
            set
            {
                Set(ref _privateMessageScrollingCollection, value);
            }
        }

        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public async void PullToRefresh_ListView(object sender, EventArgs e)
        {
            Refresh();
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

        private bool _isThreadSelectedAndLoaded;

        public bool IsThreadSelectedAndLoaded
        {
            get { return _isThreadSelectedAndLoaded; }
            set
            {
                Set(ref _isThreadSelectedAndLoaded, value);
            }
        }

        public void Refresh()
        {
            PrivateMessageScrollingCollection = new PrivateMessageScrollingCollection(WebManager);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (WebManager == null)
            {
               await LoginUser();
            }
            if (suspensionState.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<PrivateMessage>(suspensionState[nameof(Selected)]?.ToString());
                    suspensionState.Clear();
                }
            }

            if (mode == NavigationMode.Forward | mode == NavigationMode.Back)
            {
                return;
            }
            if (PrivateMessageScrollingCollection == null || !PrivateMessageScrollingCollection.Any())
            {
                Refresh();
            }
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(Selected)] = JsonConvert.SerializeObject(Selected);
            }
            return Task.CompletedTask;
        }

        public void CreateNewPm()
        {
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(NewPrivateMessagePage), JsonConvert.SerializeObject(new NewPrivateMessage()));
        }
    }
}
