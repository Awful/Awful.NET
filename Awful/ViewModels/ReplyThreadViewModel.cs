using Awful.Core.Managers;
using Awful.Parser.Models.Replies;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using Awful.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Awful.ViewModels
{
    public class ReplyThreadViewModel : NewPostBaseViewModel
    {
        public PreviousPostsViewModel PreviousPostsViewModel { get; set; }

        private ThreadReply _selected = default(ThreadReply);

        public ThreadReply Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private ForumReply _forumReply { get; set; }

        private ReplyManager _replyManager;

        public async Task Init(ThreadReply parameter)
        {
            if (WebManager == null)
            {
                LoginUser();
            }

            _replyManager = new ReplyManager(WebManager);

            Selected = parameter;

            if (Selected.IsEdit)
            {
                Title = "Edit - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookiesForEditAsync(Selected.QuoteId);
                ReplyBox.Text = _forumReply.Quote;
            }
            else if (Selected.QuoteId > 0)
            {
                Title = "Quote - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookiesAsync(0, Selected.QuoteId);
                ReplyBox.Text = _forumReply.Quote;
            }
            else
            {
                Title = "Reply - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookiesAsync(Selected.Thread.ThreadId);
            }
        }

        public async Task ReplyToThread()
        {
            if (string.IsNullOrEmpty(ReplyBox.Text) || _forumReply == null) return;
            _forumReply.Message = ReplyBox.Text;
            IsLoading = true;
            var loadingString = Selected.IsEdit ? "Editing Post..." : "Posting reply (Better hope it doesn't suck...)";
            //Views.Shell.ShowBusy(true, loadingString);
            Result result;
            if (Selected.IsEdit)
            {
                result = await _replyManager.SendUpdatePostAsync(_forumReply);
            }
            else
            {
                result = await _replyManager.SendPostAsync(_forumReply);
            }
            //Views.Shell.ShowBusy(false);
            if (result.IsSuccess)
            {
                IsLoading = false;
                NavigationService.GoBack();
                return;
            }
            IsLoading = false;
            // TODO: Add error message when something screws up.
        }

        public async Task PreviewPost()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || _forumReply == null) return;
            _forumReply.Message = ReplyBox.Text;
            PreviewViewModel.IsOpen = true;
            var result = Selected.IsEdit
                ? await _replyManager.CreatePreviewEditPostAsync(_forumReply)
                : await _replyManager.CreatePreviewPostAsync(_forumReply);
            PreviewViewModel.LoadPost(result);
            IsLoading = false;
        }

        public void ShowPreviousPosts()
        {
            PreviousPostsViewModel.IsOpen = true;
            PreviousPostsViewModel.LoadPreviousPosts(Selected.Thread, _forumReply.ForumPosts);
        }
    }
}