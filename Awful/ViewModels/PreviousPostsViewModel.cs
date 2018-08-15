using Awful.Parser.Models.Threads;
using Awful.Tools;
using Awful.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Awful.ViewModels
{
    public class PreviousPostsViewModel : AwfulViewModel
    {
        public TextBox ReplyBox { get; set; }

        public WebView Web { get; set; }

        public WebCommands WebCommands { get; set; }

        public async Task SetupWebView()
        {
           // await Web.InvokeScriptAsync("FromCSharp", ForumCommandCreator.CreateForumCommand("setupWebview", GetForumThreadSettings()));
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

        //private ReplyManager _replyManager;

        public async void LoadPreviousPosts(Thread thread, string posts)
        {
        //    if (!string.IsNullOrEmpty(PostHtml)) return;
        //    if (WebManager == null)
        //    {
        //        await LoginUser();
        //    }

        //    _replyManager = new ReplyManager(WebManager);
        //    var newPosts = JsonConvert.DeserializeObject<List<Post>>(posts);
            //PostHtml = await HtmlFormater.FormatThreadHtml(thread, newPosts, GetTheme, true, true);
        }

        public async void AddQuoteString(long postId)
        {
            //if (_replyManager == null)
            //{
            //    LoginUser();
            //}
            //var quoteString = await _replyManager.GetQuoteString(postId);
            //ReplyBox.Text += Environment.NewLine + quoteString + Environment.NewLine;
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
