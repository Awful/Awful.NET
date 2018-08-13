using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Awful.ViewModels
{
    public class NewPostBaseViewModel : AwfulViewModel
    {
        private TextBox _replyBox = default(TextBox);

        private string _title = default(string);

        public string Title
        {
            get { return _title; }
            set
            {
                Set(ref _title, value);
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
    }
}
