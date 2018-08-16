using Awful.Core.Managers;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.Tools;
using Awful.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using static Awful.ViewModels.ThreadViewModel;

namespace Awful.ViewModels
{
    public class PreviousPostsViewModel : ThreadBaseViewModel
    {
        public TextBox ReplyBox { get; set; }

        private ReplyManager _replyManager;


        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        public async void LoadPreviousPosts(Thread thread, List<Post> posts)
        {
            if (WebManager == null)
            {
                LoginUser();
            }

            _replyManager = new ReplyManager(WebManager);
            await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("addPosts", new Thread() { IsLoggedIn = true, Posts = posts }));
        }

        public async void AddQuoteString(long postId)
        {
            if (_replyManager == null)
            {
                LoginUser();
            }
            var quoteString = await _replyManager.GetQuoteStringAsync(postId);
            ReplyBox.Text += Environment.NewLine + quoteString + Environment.NewLine;
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
