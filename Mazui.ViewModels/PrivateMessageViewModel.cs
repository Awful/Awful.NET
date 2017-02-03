using Mazui.Core.Managers;
using Mazui.Core.Models.Messages;
using Mazui.Core.Models.Posts;
using Mazui.Services;
using Mazui.Views;
using Mazui.WebTemplate.Legacy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.ViewModels
{
    public class PrivateMessageViewModel : MazuiViewModel
    {
        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private string _html = default(string);

        public string Html
        {
            get { return _html; }
            set
            {
                Set(ref _html, value);
            }
        }

        private PrivateMessageManager _postManager;

        public async Task LoadPrivateMessage()
        {
            IsLoading = true;
            if (WebManager == null)
            {
                await LoginUser();
            }
            _postManager = new PrivateMessageManager(WebManager);
            var result = await _postManager.GetPrivateMessageAsync(Selected.MessageUrl);
            if (!result.IsSuccess)
            {
                IsLoading = false;
                return;
                // TODO: Show error.
            }

            var postresult = JsonConvert.DeserializeObject<Post>(result.ResultJson);

            await FormatPmHtml(postresult);
            IsLoading = false;
        }

        private async Task FormatPmHtml(Post postEntity)
        {
            var threadTemplateModel = new PrivateMessageTemplateModel
            {
                PMPost = postEntity,
                IsDarkThemeSet = SettingsService.Instance.AppTheme == Windows.UI.Xaml.ApplicationTheme.Dark
            };
            var threadTemplate = new PrivateMessageTemplate { Model = threadTemplateModel };
            Html = threadTemplate.GenerateString();
        }

        public void Reply()
        {
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(NewPrivateMessagePage),
                JsonConvert.SerializeObject(Selected));
        }
    }
}
