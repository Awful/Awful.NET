using Awful.Core.Managers;
using Awful.Parser.Models.Replies;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Awful.ViewModels
{
    public class NewPostBaseViewModel : ThreadBaseViewModel
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }
        public PreviewViewModel PreviewViewModel { get; set; }
        public PostIconViewModel PostIconViewModel { get; set; }

        private TextBox _replyBox = default(TextBox);

        private string _title = default(string);

        public string Title
        {
            set
            {
                App.ShellViewModel.Header = value;
            }
        }

        public TextBox ReplyBox
        {
            get { return _replyBox; }
            set
            {
                Set(ref _replyBox, value);
            }
        }

        private TextBox _subject = default(TextBox);

        public TextBox Subject
        {
            get { return _subject; }
            set
            {
                Set(ref _subject, value);
            }
        }

        public async Task AddImageViaImgur()
        {
            IsLoading = true;
            //await AddImage.AddImageViaImgur(ReplyBox);
            IsLoading = false;
        }

        public async void OpenSmiliesView()
        {
            await SmiliesViewModel.LoadSmiliesAsync();
            SmiliesViewModel.IsOpen = true;
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
    }
}
