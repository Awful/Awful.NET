using Awful.Parser.Models.Threads;
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

        public MasterDetailSplitViewControl MasterDetailViewControl { get; set; }

        public ThreadView()
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

        public async Task LoadBaseView()
        {
           var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Forums/index.html"));
           var html = await ParsedHtmlBase(await FileIO.ReadTextAsync(file));
           Web.NavigateToString(html);
        }

        private async Task<string> ParsedHtmlBase(string html)
        {
            var basePage = await ViewModel.WebManager.Parser.ParseAsync(html);
            var links = basePage.QuerySelectorAll("link");
            foreach(var link in links)
            {
                var attribute = link.GetAttribute("href");
                link.SetAttribute("href", $"ms-appx-web:///Assets/Forums{attribute}");
            }
            var scripts = basePage.QuerySelectorAll("script");
            foreach (var script in scripts)
            {
                var attribute = script.GetAttribute("src");
                if (attribute == null)
                    continue;
                if (attribute[0] != '/')
                    script.SetAttribute("src", $"ms-appx-web:///Assets/Forums/{attribute}");
                else
                    script.SetAttribute("src", $"ms-appx-web:///Assets/Forums{attribute}");
            }

            return "<!DOCTYPE html> " + basePage.DocumentElement.OuterHtml;
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

        public async Task LoadThread(Thread thread, bool fromSuspend = false, bool lastPage = false)
        {
            MasterDetailViewControl.SetPreviewHeaderText(thread.Name);
            ViewModel.Init();
            if (lastPage)
            {
                thread.CurrentPage = thread.TotalPages;
                thread.RepliesSinceLastOpened = 0;
            }
            else if (thread.CurrentPage == 0)
            {
                thread.CurrentPage = 1;
            }
            if (fromSuspend && ViewModel.Selected != null)
            {
                return;
            }
            ViewModel.Selected = thread;
            await ViewModel.ReloadThread(!lastPage);
            ViewModel.IsPageLoaded = true;
        }

        private async void ScrollToBottom(object sender, RoutedEventArgs e)
        {
            try
            {
               // await Web.InvokeScriptAsync("ScrollToBottom", null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void PageNumberTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int n;
            bool isNumeric = int.TryParse(PageNumberTextBox.Text, out n);
            if (isNumeric)
            {
                ViewModel.PageSelection = PageNumberTextBox.Text;
            }
        }

        private async void ThreadPageHeader_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                //await Web.InvokeScriptAsync("ScrollToTop", null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
