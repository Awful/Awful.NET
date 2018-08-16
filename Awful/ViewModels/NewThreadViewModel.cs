using Awful.Parser.Managers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Awful.ViewModels
{
    public class NewThreadViewModel : NewPostBaseViewModel
    {

        private Forum _selected = default(Forum);

        public Forum Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private NewThread _newThread;

        private ThreadManager _threadManager;

        public async void OpenPostIconView()
        {
            await PostIconViewModel.Initialize(Selected.ForumId);
            PostIconViewModel.IsOpen = true;
        }

        public async Task Init(Forum parameter)
        {
            if (WebManager == null)
            {
                LoginUser();
            }

            _threadManager = new ThreadManager(WebManager);
            Selected = parameter;

            Title = "New Thread - " + Selected.Name;

            _newThread = await _threadManager.GetThreadCookiesAsync(Selected.ForumId);
        }

        public async Task CreateThreadAsync()
        {
            if (PostIconViewModel.PostIcon == null) return;
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || string.IsNullOrEmpty(Subject.Text) || _newThread == null) return;
            _newThread.Content = ReplyBox.Text;
            _newThread.Subject = Subject.Text;
            _newThread.PostIcon = PostIconViewModel.PostIcon;
            _newThread.ForumId = Selected.ForumId;
            var result = await _threadManager.CreateNewThreadAsync(_newThread);
            if (result.IsSuccess)
            {
                IsLoading = false;
                NavigationService.GoBack();
                return;
            }

            IsLoading = false;

            // TODO: Add error message when something screws up.
        }

        public async Task PreviewThread()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || _newThread == null) return;
            _newThread.Content = ReplyBox.Text;
            _newThread.Subject = Subject.Text;
            _newThread.PostIcon = PostIconViewModel.PostIcon;
            _newThread.ForumId = Selected.ForumId;
            PreviewViewModel.IsOpen = true;
            var result = await _threadManager.CreateNewThreadPreviewAsync(_newThread);
            PreviewViewModel.LoadPost(result);
            IsLoading = false;
        }
    }
}
