using Awful.Models.Forums;
using Awful.Models.Posts;
using Awful.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.ViewModels
{
    public class ForumMasterDetailViewModel : AwfulViewModel
    {
        private ThreadPosts _selected;

        public ThreadPosts Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

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
    }
}
