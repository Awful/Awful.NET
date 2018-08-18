using Awful.Helpers;
using Awful.Parser.Models.Threads;
using Awful.Tools;
using Awful.ViewModels;
using Awful.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task LoadThread(Thread thread, bool fromSuspend = false, bool lastPage = false)
        {
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
            if (MasterDetailViewControl != null)
                MasterDetailViewControl.SetPreviewHeaderText(thread.Name);
            else
                App.ShellViewModel.Header = thread.Name;
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

        private async void Web_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            Debug.WriteLine(args.Uri);
            args.Handled = true;
            await ViewModel.HandleLinks(args.Uri);
        }

        private void WebView_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            args.PermissionRequest.Allow();
        }
    }
}
