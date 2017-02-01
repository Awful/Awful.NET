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
using Mazui.Controls;
using Mazui.Core.Tools;

namespace Mazui.ViewModels
{
    public class ThreadPageViewModel : MazuiViewModel
    {
        public ThreadView ThreadView { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await ThreadView.ViewModel.Init();
            var parameterPassed = parameter as string;
            if (parameterPassed != null)
            {
                var thread = JsonConvert.DeserializeObject<Thread>(parameterPassed);
                ThreadView.ViewModel.Selected = thread;
                await ThreadView.ViewModel.LoadThread();
                return;
            }

            await ThreadView.ViewModel.LoadFromState(state);
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                if (ThreadView.ViewModel.Selected != null)
                {
                    var newThread = ThreadView.ViewModel.Selected.Clone();
                    newThread.Html = null;
                    newThread.Posts = null;
                    state[EndPoints.SavedThread] = JsonConvert.SerializeObject(newThread);
                }
            }
            return Task.CompletedTask;
        }
    }
}
