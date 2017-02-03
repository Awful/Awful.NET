using Mazui.Core.Managers;
using Mazui.Core.Models.Posts;
using Mazui.Core.Models.Threads;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Mazui.ViewModels
{
    public class PreviousPostsViewModel : MazuiViewModel
    {
        public TextBox ReplyBox { get; set; }

        private string _postHtml = default(string);

        public string PostHtml
        {
            get { return _postHtml; }
            set
            {
                Set(ref _postHtml, value);
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

        private WebManager _webManager;

        private ReplyManager _replyManager;

        public async void LoadPreviousPosts(Thread thread, string posts)
        {
            if (!string.IsNullOrEmpty(PostHtml)) return;
            if (WebManager == null)
            {
                await LoginUser();
            }

            _replyManager = new ReplyManager(WebManager);
            var newPosts = JsonConvert.DeserializeObject<List<Post>>(posts);
            //PostHtml = await HtmlFormater.FormatThreadHtml(thread, newPosts, GetTheme, true, true);
        }

        public async void AddQuoteString(long postId)
        {
            if (_replyManager == null)
            {
                await LoginUser();
            }
            var quoteString = await _replyManager.GetQuoteString(postId);
            ReplyBox.Text += Environment.NewLine + quoteString + Environment.NewLine;
        }
    }
}
