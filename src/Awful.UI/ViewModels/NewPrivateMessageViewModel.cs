using Awful.Entities;
using Awful.Entities.PostIcons;
using Awful.UI.Actions;
using Awful.UI.Controls;
using Awful.UI.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.UI.ViewModels
{
    public class NewPrivateMessageViewModel : ThreadPostBaseViewModel
    {
        private PrivateMessageActions pmActions;
        private PostIcon? postIcon;
        private string to = string.Empty;
        private AsyncCommand? postPMCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewPrivateMessageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public NewPrivateMessageViewModel(IAwfulEditor editor, IServiceProvider services)
            : base(editor, services)
        {
            pmActions = new PrivateMessageActions(Client, Context, Templates);
        }

        /// <summary>
        /// Gets or sets the subject of the post.
        /// </summary>
        public string To
        {
            get
            {
                return to;
            }

            set
            {
                SetProperty(ref to, value);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Post Icon.
        /// </summary>
        public PostIcon? PostIcon
        {
            get
            {
                return postIcon;
            }

            set
            {
                SetProperty(ref postIcon, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can post a pm.
        /// </summary>
        public bool CanPostPm
        {
            get { return !string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(To) && !string.IsNullOrEmpty(Message); }
        }

        /// <summary>
        /// Gets the SelectPostIcon Command.
        /// </summary>
        public AsyncCommand SelectPostIconCommand
        {
            get
            {
                return new AsyncCommand(
                    () =>
                    {
                        //if (this.popup != null)
                        //{
                        //    this.popup.SetContent(new ForumPostIconSelectionView(null, this.PostIcon), true, this.OnCloseModal);
                        //}

                        return Task.CompletedTask;
                    },
                    null,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets the post pm command.
        /// </summary>
        public AsyncCommand PostThreadCommand
        {
            get
            {
                return postPMCommand ??= new AsyncCommand(
                    async () =>
                    {
                        //if (this.newPrivateMessage != null)
                        //{
                        //    var saItem = await this.SendPrivateMessageAsync().ConfigureAwait(false);

                        //    // If we get a null result, we couldn't post at all. Ignore.
                        //    // TODO: It shouldn't ever return null anyway because of the Command Check.
                        //    // We probably don't need this check.
                        //    if (saItem == null)
                        //    {
                        //        return;
                        //    }

                        //    if (saItem.IsResultSet && saItem.Result.IsSuccess)
                        //    {
                        //        //MainThread.BeginInvokeOnMainThread(async () =>
                        //        //{
                        //        //    await this.Navigation.PopModalAsync().ConfigureAwait(false);
                        //        //});
                        //    }
                        //}
                    },
                    () => CanPostPm,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            PostThreadCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Sends the private message.
        /// </summary>
        /// <returns>Result.</returns>
        public async Task<SAItem?> SendPrivateMessageAsync()
        {
            var pmText = Message.Trim();
            if (string.IsNullOrEmpty(pmText))
            {
                return null;
            }

            var pmTitle = Subject.Trim();
            if (string.IsNullOrEmpty(pmTitle))
            {
                return null;
            }

            var to = To.Trim();
            if (string.IsNullOrEmpty(to))
            {
                return null;
            }

            // The Manager will throw if we couldn't post.
            // That will be captured by AwfulAsyncCommand.
            // return await this.pmActions.SendPrivateMessageAsync(this.newPrivateMessage).ConfigureAwait(false);
            return null;
        }
    }
}
