using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Mazui.ViewModels
{
    public class NewPostBaseViewModel : MazuiViewModel
    {
        private TextBox _replyBox = default(TextBox);

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
    }
}
