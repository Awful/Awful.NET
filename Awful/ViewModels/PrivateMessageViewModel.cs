using Awful.Parser.Managers;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using Awful.Tools;
using Awful.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Awful.Services;
using System.IO;
using Windows.UI.Xaml.Controls;
using Awful.Web;
using Awful.Parser.Models.Messages;
using static Awful.ViewModels.ThreadViewModel;
using Awful.Controls;

namespace Awful.ViewModels
{
    public class PrivateMessageViewModel : ThreadBaseViewModel
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

        private string _pageSelection;

        public string PageSelection
        {
            get { return _pageSelection; }
            set
            {
                Set(ref _pageSelection, value);
            }
        }
        private PrivateMessageManager _postManager;

        #endregion

        public void Init()
        {
            LoginUser();
            _postManager = new PrivateMessageManager(WebManager);
        }

        public async Task LoadPrivateMessage()
        {
            IsLoading = true;
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("reset", null));
            var postresult = await _postManager.GetPrivateMessageAsync(Selected);
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", new Thread() { IsLoggedIn = false, Posts = new List<Post>() { postresult } } ));
            OnPropertyChanged("Selected");
            IsLoading = false;
        }

        public void Reply()
        {

        }

        internal async Task HandleForumCommand(ForumCommand test)
        {
            switch (test.Type)
            {
                default:
                    break;
            }
        }
    }
}
