using Mazui.Core.Managers;
using Mazui.Core.Models.Posts;
using Mazui.Core.Models.Replies;
using Mazui.Core.Models.Threads;
using Mazui.Core.Models.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Mazui.ViewModels
{
    public class ReplyThreadViewModel : NewPostBaseViewModel
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }
        public PreviewViewModel PreviewViewModel { get; set; }
        public PreviousPostsViewModel PreviousPostsViewModel { get; set; }

        public async void OpenSmiliesView()
        {
            await SmiliesViewModel.LoadSmilies();
            SmiliesViewModel.IsOpen = true;
        }

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

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            await base.OnNavigatedToAsync(parameter, mode, suspensionState);

            if (WebManager == null)
            {
                await LoginUser();
            }

            _replyManager = new ReplyManager(WebManager);
            Selected = JsonConvert.DeserializeObject<ThreadReply>(parameter.ToString());
            //Views.Shell.ShowBusy(true, "Preparing thread...");
            if (Selected.IsEdit)
            {
                Title = "Edit - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookiesForEdit(Selected.QuoteId);
                ReplyBox.Text = _forumReply.Quote;
            }
            else if (Selected.QuoteId > 0)
            {
                Title = "Quote - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookies(0, Selected.QuoteId);
                ReplyBox.Text = _forumReply.Quote;
            }
            else
            {
                Title = "Reply - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookies(Selected.Thread.ThreadId);
            }
            //Views.Shell.ShowBusy(false);
        }

        public void SelectBbCode(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem == null) return;
            var code = "";
            if (menuFlyoutItem.CommandParameter != null)
            {
                switch (menuFlyoutItem.CommandParameter.ToString().ToLower())
                {
                    case "bold":
                        code = "b";
                        break;
                    case "indent":
                        code = "i";
                        break;
                    case "strike":
                        code = "s";
                        break;
                    case "spoiler":
                        code = "spoiler";
                        break;
                    case "quote":
                        code = "quote";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(ReplyBox.SelectedText))
            {
                string selectedText = "[{0}]" + ReplyBox.SelectedText + "[/{0}]";
                ReplyBox.SelectedText = string.Format(selectedText, code);
            }
            else
            {
                string text = string.Format("[{0}][/{0}]", code);
                string replyText = string.IsNullOrEmpty(ReplyBox.Text) ? string.Empty : ReplyBox.Text;
                if (replyText != null) ReplyBox.Text = replyText.Insert(ReplyBox.SelectionStart, text);
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
                result = await _replyManager.SendUpdatePost(_forumReply);
            }
            else
            {
                result = await _replyManager.SendPost(_forumReply);
            }
            //Views.Shell.ShowBusy(false);
            if (result.IsSuccess)
            {
                IsLoading = false;
                Template10.Common.BootStrapper.Current.NavigationService.GoBack();
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
                ? await _replyManager.CreatePreviewEditPost(_forumReply)
                : await _replyManager.CreatePreviewPost(_forumReply);
            var post = JsonConvert.DeserializeObject<Post>(result.ResultJson);
            PreviewViewModel.LoadPost(Selected.Thread, post);
            IsLoading = false;
        }

        public void ShowPreviousPosts()
        {
            PreviousPostsViewModel.IsOpen = true;
            PreviousPostsViewModel.LoadPreviousPosts(Selected.Thread, JsonConvert.SerializeObject(_forumReply.ForumPosts));
        }
    }
}
