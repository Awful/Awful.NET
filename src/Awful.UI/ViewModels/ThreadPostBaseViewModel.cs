using Awful.UI.Actions;
using Awful.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Thread Post Base View Model.
    /// </summary>
    public class ThreadPostBaseViewModel : AwfulViewModel
    {
        protected ThreadReplyActions replyActions;
        protected ThreadPostCreationActions postActions;
        protected ThreadActions threadActions;
        private string subject = string.Empty;
        private string message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostBaseViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public ThreadPostBaseViewModel(IAwfulEditor editor, IServiceProvider services)
            : base(services)
        {
            Editor = editor;
            replyActions = new ThreadReplyActions(Client, Context, Templates);
            postActions = new ThreadPostCreationActions(Client);
            threadActions = new ThreadActions(Client, Context);
        }

        /// <summary>
        /// Gets the thread editor.
        /// </summary>
        public IAwfulEditor Editor { get; }

        /// <summary>
        /// Gets or sets the subject of the post.
        /// </summary>
        public string Subject
        {
            get
            {
                return subject;
            }

            set
            {
                SetProperty(ref subject, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the message of the post.
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                SetProperty(ref message, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <inheritdoc/>
        public override Task OnLoad()
        {
            if (Editor != null)
            {
                Editor.Focus();
            }

            return base.OnLoad();
        }
    }
}
