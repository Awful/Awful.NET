using Mazui.Core.Managers;
using Mazui.Core.Models.Messages;
using Mazui.Core.Models.Web;
using Mazui.Tools.Web;
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
    public class NewPrivateMessageViewModel : NewPostBaseViewModel
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }

        public PostIconViewModel PostIconViewModel { get; set; }

        private string _title = default(string);

        public string Title
        {
            get { return _title; }
            set
            {
                Set(ref _title, value);
            }
        }

        public async void OpenSmiliesView()
        {
            await SmiliesViewModel.LoadSmilies();
            SmiliesViewModel.IsOpen = true;
        }

        private TextBox _recipient = default(TextBox);

        public TextBox Recipient
        {
            get { return _recipient; }
            set
            {
                Set(ref _recipient, value);
            }
        }

        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private NewPrivateMessage _newPrivateMessage = new NewPrivateMessage();

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            if (WebManager == null)
            {
                await LoginUser();
            }
            _postManager = new PrivateMessageManager(WebManager);
            await base.OnNavigatedToAsync(parameter, mode, suspensionState);
            Selected = JsonConvert.DeserializeObject<PrivateMessage>(parameter.ToString());
            if (!string.IsNullOrEmpty(Selected.Title))
            {
                Title = "Re: " + Selected.Title;
                Subject.Text = "Re: " + Selected.Title;
            }
            else
            {
                Title = "New Private Message";
            }

            if (!string.IsNullOrEmpty(Selected.Sender))
            {
                Recipient.Text = Selected.Sender;
            }
            IsLoading = false;
        }

        public async Task CreatePm()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || _newPrivateMessage == null) return;
            if (PostIconViewModel.PostIcon == null) return;
            IsLoading = true;
            Result result = new Result();
            try
            {
                _newPrivateMessage.Icon = PostIconViewModel.PostIcon;
                _newPrivateMessage.Body = ReplyBox.Text;
                _newPrivateMessage.Receiver = Recipient.Text;
                _newPrivateMessage.Title = Subject.Text;
                result = await _postManager.SendPrivateMessageAsync(_newPrivateMessage);
            }
            catch (Exception)
            {
                // TODO: Show error.
            }
            IsLoading = false;
            if (result.IsSuccess)
            {
                Template10.Common.BootStrapper.Current.NavigationService.GoBack();
                IsLoading = false;
                return;
            }
            IsLoading = false;
            // TODO: Add error message when something screws up.
        }

        private PrivateMessageManager _postManager;

        public async void OpenPostIconView()
        {
            await PostIconViewModel.Initialize(1);
            PostIconViewModel.IsOpen = true;
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

        public async Task AddImageViaImgur()
        {
            IsLoading = true;
            await AddImage.AddImageViaImgur(ReplyBox);
            IsLoading = false;
        }
    }
}
