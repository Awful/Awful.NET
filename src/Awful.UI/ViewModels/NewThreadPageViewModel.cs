using Awful.Entities.PostIcons;
using Awful.Entities.Web;
using Awful.UI.Controls;
using Awful.UI.Entities;
using Awful.UI.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.UI.ViewModels
{
    public class NewThreadPageViewModel : ThreadPostBaseViewModel
    {
        private AwfulForum forum;
        private AsyncCommand? postThreadCommand;
        private PostIcon? postIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewThreadPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public NewThreadPageViewModel(AwfulForum forum, IAwfulEditor editor, IServiceProvider services)
            : base(editor, services)
        {
            this.forum = forum;
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
        /// Gets the post thread command.
        /// </summary>
        public AsyncCommand PostThreadCommand
        {
            get
            {
                return postThreadCommand ??= new AsyncCommand(
                    async () =>
                    {
                        //if (this.newThread != null)
                        //{

                        //    var result = await this.PostNewThreadAsync().ConfigureAwait(false);

                        //    // If we get a null result, we couldn't post at all. Ignore.
                        //    // TODO: It shouldn't ever return null anyway because of the Command Check.
                        //    // We probably don't need this check.
                        //    if (result == null)
                        //    {
                        //        return;
                        //    }

                        //    if (result.IsSuccess)
                        //    {
                        //        MainThread.BeginInvokeOnMainThread(async () =>
                        //        {
                        //            await this.Navigation.PopModalAsync().ConfigureAwait(false);
                        //            await this.Navigation.RefreshForumPageAsync().ConfigureAwait(false);
                        //        });
                        //    }
                        //}
                    },
                    () => CanPost,
                    Dispatcher,
                    ErrorHandler);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can post.
        /// </summary>
        public bool CanPost
        {
            get { return !string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Message) && !string.IsNullOrEmpty(postIcon?.ImageEndpoint); }
        }

        public override async Task OnLoad()
        {
            await base.OnLoad();

            //if (this.newThread == null)
            //{
            //    this.newThread = await this.threadActions.CreateNewThreadAsync(this.forum.Id).ConfigureAwait(false);
            //}
            //else
            //{
            //    this.OnPropertyChanged(nameof(this.PostIcon));
            //}
        }

        /// <inheritdoc/>
        public override void RaiseCanExecuteChanged()
        {
            PostThreadCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Posts the thread to the forums.
        /// </summary>
        /// <returns>A Result.</returns>
        public async Task<Result?> PostNewThreadAsync()
        {
            //var threadText = this.Message.Trim();
            //if (string.IsNullOrEmpty(threadText))
            //{
            //    return null;
            //}

            //var threadTitle = this.Subject.Trim();
            //if (string.IsNullOrEmpty(threadTitle))
            //{
            //    return null;
            //}

            //if (string.IsNullOrEmpty(this.PostIcon.ImageLocation))
            //{
            //    return null;
            //}

            //this.newThread.PostIcon = this.PostIcon;
            //this.newThread.Subject = threadTitle;
            //this.newThread.Content = threadText;

            //// The Manager will throw if we couldn't post.
            //// That will be captured by AwfulAsyncCommand.
            //return await this.threadActions.PostNewThreadAsync(this.newThread).ConfigureAwait(false);

            return null;
        }
    }
}
