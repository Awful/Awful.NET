using Mazui.Core.Models.Posts;
using Mazui.Core.Models.Threads;
using Mazui.Services;
using Mazui.WebTemplate.Legacy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazui.ViewModels
{
    public class PreviewViewModel : MazuiViewModel
    {
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

        public async void LoadPost(Thread thread, Post post)
        {
            var threadTemplateModel = new PreviewPostTemplateModel
            {
                Post = post,
                IsDarkThemeSet = SettingsService.Instance.AppTheme == Windows.UI.Xaml.ApplicationTheme.Dark
            };
            var threadTemplate = new PreviewPostTemplate { Model = threadTemplateModel };
            PostHtml = threadTemplate.GenerateString();
        }
    }
}
