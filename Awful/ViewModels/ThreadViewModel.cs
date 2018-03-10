using Awful.Managers;
using Awful.Models.Posts;
using Awful.Models.Threads;
using Awful.Models.Web;
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

namespace Awful.ViewModels
{
    public class ThreadViewModel : AwfulViewModel
    {
        #region Properties

        private bool _hasReactLoaded = default(bool);

        public bool HasReactLoaded
        {
            get { return _hasReactLoaded; }
            set
            {
                Set(ref _hasReactLoaded, value);
            }
        }

        private Thread _selected = default(Thread);

        public Thread Selected
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
        private PostManager _postManager;
        #endregion

        public void Init()
        {
            if (WebManager == null)
            {
                LoginUser();
            }

            if (_postManager == null) _postManager = new PostManager(WebManager);
        }

        public async Task HandleForumCommand(ForumCommand forumCommand)
        {
            switch(forumCommand.Type)
            {
                case "reactLoaded":
                    HasReactLoaded = true;
                    break;
                default:
                    break;
            }
        }
    }
}
