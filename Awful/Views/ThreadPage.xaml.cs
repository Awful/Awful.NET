using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Awful.Views
{
    public sealed partial class ThreadPage : Page
    {
        public ThreadPageViewModel ViewModel => this.DataContext as ThreadPageViewModel;

        public ThreadPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel.ThreadView = ThreadPageView;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
           // await ViewModel.SetupWebView();
           // await ViewModel.Init(JsonConvert.DeserializeObject<Thread>(e.Parameter as string));
        }
    }
}
