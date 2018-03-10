using Awful.Tools;
using Awful.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Awful.Controls
{
    public sealed partial class ThreadView : UserControl
    {
        public static ThreadView Instance { get; set; }

        public ThreadViewModel ViewModel => this.DataContext as ThreadViewModel;

        public ThreadView()
        {
            this.InitializeComponent();
        }

        public async Task LoadBaseView()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Forums/WebPage.html"));
            Web.NavigateToString(await FileIO.ReadTextAsync(file));
        }
    
        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {

        }

        private async void Web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //await Web.InvokeScriptAsync("app.props.appState.addTestPosts", new string[0]);
        }

        private async void Web_ScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                var test = JsonConvert.DeserializeObject<ForumCommand>(e.Value);
                await ViewModel.HandleForumCommand(test);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
