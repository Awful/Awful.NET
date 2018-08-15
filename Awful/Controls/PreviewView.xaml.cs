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
    public sealed partial class PreviewView : UserControl
    {
        public PreviewView()
        {
            this.InitializeComponent();
            ViewModel.LoginUser();
            ViewModel.Web = Web;
            Web.LoadCompleted += Web_LoadCompleted;
            LoadBaseView();
        }

        public PreviewViewModel ViewModel => this.DataContext as PreviewViewModel;

        private async void Web_LoadCompleted(object sender, NavigationEventArgs e)
        {
            await ViewModel.SetupWebView();
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
            foreach (var link in links)
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
    }
}
