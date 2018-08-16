using Awful.Parser.Managers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Messages;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using Awful.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Awful.ViewModels
{
    public class NewPrivateMessageViewModel : NewPostBaseViewModel
    {
        private TextBox _recipient = default(TextBox);

        public TextBox Recipient
        {
            get { return _recipient; }
            set
            {
                Set(ref _recipient, value);
            }
        }

        public async void OpenPostIconView()
        {
            await PostIconViewModel.Initialize(0);
            PostIconViewModel.IsOpen = true;
        }

        private PrivateMessageManager _postManager;

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

        public async Task Init(PrivateMessage parameter)
        {
            IsLoading = true;
            if (WebManager == null)
            {
                LoginUser();
            }
            _postManager = new PrivateMessageManager(WebManager);
            Selected = parameter;
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

        public async Task CreatePmAsync()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || _newPrivateMessage == null) return;
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
                NavigationService.GoBack();
                IsLoading = false;
                return;
            }
            IsLoading = false;
            // TODO: Add error message when something screws up.
        }
    }
}
