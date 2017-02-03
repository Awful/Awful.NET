using Mazui.Core.Managers;
using Mazui.Core.Models.PostIcons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI.Xaml.Controls;

namespace Mazui.ViewModels
{
    public class PostIconViewModel : MazuiViewModel
    {
        private ObservableCollection<PostIcon> _postIconEntities = new ObservableCollection<PostIcon>();

        public ObservableCollection<PostIcon> PostIconEntities
        {
            get { return _postIconEntities; }
            set
            {
                Set(ref _postIconEntities, value);
            }
        }

        private PostIconManager _postIconManager;

        private PostIcon _postIcon = default(PostIcon);

        public PostIcon PostIcon
        {
            get { return _postIcon; }
            set
            {
                Set(ref _postIcon, value);
            }
        }

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        public void SelectIcon(object sender, ItemClickEventArgs e)
        {
            var smile = e.ClickedItem as PostIcon;
            if (smile == null) return;
            PostIcon = smile;
            IsOpen = false;
        }

        public async Task Initialize(int forumId)
        {
            if (WebManager == null)
            {
                await LoginUser();
            }

            _postIconManager = new PostIconManager(WebManager);

            if ((PostIconEntities == null || !PostIconEntities.Any()))
            {
                var test = await _postIconManager.GetPostIcons(forumId);
                PostIconEntities = test.First().List.ToObservableCollection();
            }
            else if (forumId == 0)
            {
                var test = await _postIconManager.GetPmPostIcons();
                PostIconEntities = test.First().List.ToObservableCollection();
            }
        }
    }
}
