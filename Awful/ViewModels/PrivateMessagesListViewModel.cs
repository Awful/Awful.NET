using Awful.Controls;
using Awful.Parser.Models.Messages;
using Awful.Services;
using Awful.Tools;
using Awful.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.ViewModels
{
    public class PrivateMessagesListViewModel : AwfulViewModel
    {
        #region Properties

        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
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

        public MasterDetailSplitViewControl MasterDetailViewControl { get; set; }

        public PrivateMessageView ThreadView { get; set; }

        private PrivateMessageScrollingCollection _privateMessageScrollingCollection = default(PrivateMessageScrollingCollection);
        public PrivateMessageScrollingCollection PrivateMessageScrollingCollection
        {
            get { return _privateMessageScrollingCollection; }
            set
            {
                Set(ref _privateMessageScrollingCollection, value);
            }
        }

        #endregion

        #region Methods

        public void Init()
        {
            LoginUser();
            PrivateMessageScrollingCollection = new PrivateMessageScrollingCollection(WebManager);
        }

        public void CreatePM()
        {
           NavigationService.Navigate(typeof(NewPrivateMessagePage), JsonConvert.SerializeObject(new NewPrivateMessage()));
        }

        #endregion
    }
}
