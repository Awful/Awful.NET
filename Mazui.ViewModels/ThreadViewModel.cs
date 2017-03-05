using Mazui.Core.Managers;
using Mazui.Core.Models.Posts;
using Mazui.Core.Models.Threads;
using Mazui.Core.Models.Web;
using Mazui.Tools;
using Mazui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using System.Linq;
using Mazui.WebTemplate.Legacy;
using Mazui.Services;
using System.IO;
using Mazui.Core.Tools;

namespace Mazui.ViewModels
{
    public class ThreadViewModel : MazuiViewModel
    {
        #region Properties
        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private string _pageSelection;

        public string PageSelection
        {
            get { return _pageSelection; }
            set
            {
                Set(ref _pageSelection, value);
            }
        }
        private PostManager _postManager;
        #endregion
        
        public async Task Init()
        {
            if (WebManager == null)
            {
                await LoginUser();
            }

            if (_postManager == null) _postManager = new PostManager(WebManager);
        }

        public async Task LoadFromState(IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(EndPoints.SavedThread))
            {
                Selected = JsonConvert.DeserializeObject<Thread>(suspensionState[EndPoints.SavedThread]?.ToString());
                await LoadThread(false);
            }
        }

        #region Methods

        public async Task LoadThread(bool goToPageOverride = false)
        {
            IsLoading = true;
            var result = await _postManager.GetThreadPostsAsync(Selected.Location, Selected.CurrentPage, Selected.HasBeenViewed, goToPageOverride);
            if (await CheckResult(result) == false) return;
            var threadPosts = await SetupPosts(result);
            await SetupHtml(threadPosts);
            IsLoading = false;
        }

        public async Task NextPage()
        {
            if (Selected.CurrentPage >= Selected.TotalPages) return;
            Selected.CurrentPage++;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await LoadThread();
        }

        public async Task PreviousPage()
        {
            if (Selected.CurrentPage <= 0) return;
            Selected.CurrentPage--;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await LoadThread();
        }

        public async Task ReloadThread()
        {
            await LoadThread();
        }

        public async Task FirstThreadPage()
        {
            Selected.CurrentPage = 1;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public async Task LastThreadPage()
        {
            Selected.CurrentPage = Selected.TotalPages;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public async Task ChangeThreadPage()
        {
            int userInputPageNumber;
            try
            {
                userInputPageNumber = Convert.ToInt32(PageSelection);
            }
            catch (Exception)
            {
                // User entered invalid number, return.
                return;
            }

            if (userInputPageNumber < 1 || userInputPageNumber > Selected.TotalPages) return;
            Selected.CurrentPage = userInputPageNumber;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public void ReplyToThread()
        {
			var reply = JsonConvert.SerializeObject(new ThreadReply()
			{
				Thread = new Thread()
				{
					ForumId = Selected.ForumId,
					ThreadId = Selected.ThreadId,
					Name = Selected.Name,
					CurrentPage = Selected.CurrentPage,
					TotalPages = Selected.TotalPages
				}
			});
			if (App.IsTenFoot)
			{
				Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(XboxViews.ReplyPage), reply);
			} else
			{
				Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(ReplyPage), reply);
			}
        }

        public async Task AddRemoveBookmarkView()
        {
            await this.AddRemoveBookmark(Selected);
        }

        public async Task AddRemoveNotificationTable()
        {
            await this.AddRemoveNotification(Selected);
        }

        private async Task SetupHtml(ThreadPosts postresult)
        {
            if (!Selected.Posts.Any()) return;
            var threadTemplateModel = new ThreadTemplateModel()
            {
                ForumThread = Selected,
                IsDarkThemeSet = SettingsService.Instance.AppTheme == Windows.UI.Xaml.ApplicationTheme.Dark,
                IsLoggedIn = IsLoggedIn,
                Posts = Selected.Posts,
                EmbeddedGifv = SettingsService.Instance.ShowEmbeddedGifv,
                EmbeddedTweets = SettingsService.Instance.ShowEmbeddedTweets,
                EmbeddedVideo = SettingsService.Instance.ShowEmbeddedVideo
            };
            var threadTemplate = new ThreadTemplate() { Model = threadTemplateModel };
            var html = threadTemplate.GenerateString();
            if (!SettingsService.Instance.AutoplayGif)
            {
				html = Mazui.Core.Tools.Extensions.RemoveAutoplayGif(html);
			}
            Selected.Html = html;
            var count = postresult.Posts.Count(node => !node.HasSeen);
            if (Selected.RepliesSinceLastOpened > 0)
            {
                if ((Selected.RepliesSinceLastOpened - count < 0) || count == 0)
                {
                    Selected.RepliesSinceLastOpened = 0;
                }
                else
                {
                    Selected.RepliesSinceLastOpened -= count;
                }
            }
            Selected.Name = postresult.ForumThread.Name;
            if (Selected.IsBookmark)
            {
                // await _db.RefreshBookmark(Selected);
            }
            RaisePropertyChanged("Selected");
        }

        private async Task<ThreadPosts> SetupPosts(Result result)
        {
            var errorMessage = "";
            try
            {
                var postresult = JsonConvert.DeserializeObject<ThreadPosts>(result.ResultJson);
                Selected.LoggedInUserName = postresult.ForumThread.LoggedInUserName;
                Selected.CurrentPage = postresult.ForumThread.CurrentPage;
                Selected.TotalPages = postresult.ForumThread.TotalPages;
                Selected.Posts = postresult.Posts;

                // If the user is the "Test" user, say they are not logged in (even though they are)
                if (Selected.LoggedInUserName == "Testy Susan")
                {
                    IsLoggedIn = false;
                }
                return postresult;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            await ResultChecker.SendMessageDialogAsync($"Error parsing thread: {errorMessage}", false);
            return null;
        }

        private async Task<bool> CheckResult(Result result)
        {
            var resultCheck = await ResultChecker.CheckPaywallOrSuccess(result);
            if (!resultCheck)
            {
                if (result.Type == typeof(Error).ToString())
                {
                    var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
                    if (error.IsPaywall)
                    {
                        Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(PaywallPage));
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
