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

namespace Mazui.ViewModels
{
    public class ThreadPageViewModel : MazuiViewModel
    {
        public ThreadView ThreadView { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var parameterPassed = parameter as string;
            if (parameterPassed != null)
            {
                var thread = JsonConvert.DeserializeObject<Thread>(parameterPassed);
                ThreadView.ViewModel.Selected = thread;
                await ThreadView.ViewModel.Init();
                await ThreadView.ViewModel.LoadThread();
            }
        }
    }
}
