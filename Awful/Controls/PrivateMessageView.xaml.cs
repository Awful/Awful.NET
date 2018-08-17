using Awful.Parser.Models.Messages;
using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.Tools;
using Awful.ViewModels;
using Awful.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Awful.Controls
{
    public sealed partial class PrivateMessageView : UserControl
    {
        public PrivateMessageViewModel ViewModel => this.DataContext as PrivateMessageViewModel;

        public MasterDetailSplitViewControl MasterDetailViewControl { get; set; }

        public PrivateMessageView()
        {
            this.InitializeComponent();
            ViewModel.Web = Web;
            Web.LoadCompleted += Web_LoadCompleted;
            ViewModel.Init();
        }

        private async void Web_LoadCompleted(object sender, NavigationEventArgs e)
        {
            await ViewModel.SetupWebView();
        }

        public async Task LoadPrivateMessage(PrivateMessage thread)
        {
            MasterDetailViewControl.SetPreviewHeaderText(thread.Title);
            ViewModel.Selected = thread;
            await ViewModel.LoadPrivateMessage();
        }

        public async Task LoadBaseView()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Forums/index.html"));
            var html = await ViewModel.ParsedHtmlBase(await FileIO.ReadTextAsync(file));
            Web.NavigateToString(html);
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            ViewModel.WebCommands = new WebCommands();
            sender.AddWebAllowedObject("webCommands", ViewModel.WebCommands);
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
